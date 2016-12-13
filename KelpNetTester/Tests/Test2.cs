﻿using System;
using KelpNet;
using KelpNet.Common;
using KelpNet.Functions.Activations;
using KelpNet.Functions.Connections;
using KelpNet.Loss;
using KelpNet.Optimizers;

namespace KelpNetTester.Tests
{
    //MLPによるXORの学習【回帰版】 ※精度が悪く何度か実行しないと望んだ結果を得られない
    class Test2
    {
        public static void Run()
        {
            //訓練回数
            const int learningCount = 10000;

            //訓練データ
            double[][] trainData = 
            {
                new[] { 0.0, 0.0 },
                new[] { 1.0, 0.0 },
                new[] { 0.0, 1.0 },
                new[] { 1.0, 1.0 }
            };

            //訓練データラベル
            double[][] trainLabel = 
            {
                new[] { 0.0 },
                new[] { 1.0 },
                new[] { 1.0 },
                new[] { 0.0 }
            };

            //ネットワークの構成を FunctionStack に書き連ねる
            FunctionStack nn = new FunctionStack(
                new Linear(2, 2, name: "l1 Linear", isParallel: false),
                new ReLU(name: "l1 ReLU", isParallel: false),
                new Linear(2, 1, name: "l2 Linear", isParallel: false)
            );

            //optimizerを宣言(今回はAdam)
            nn.SetOptimizer(new Adam());

            //訓練ループ
            Console.WriteLine("Training...");
            for (int i = 0; i < learningCount; i++)
            {
                //今回はロス関数にMeanSquaredErrorを使う
                Trainer.Train(nn, trainData[0], trainLabel[0], new MeanSquaredError(), false);
                Trainer.Train(nn, trainData[1], trainLabel[1], new MeanSquaredError(), false);
                Trainer.Train(nn, trainData[2], trainLabel[2], new MeanSquaredError(), false);
                Trainer.Train(nn, trainData[3], trainLabel[3], new MeanSquaredError(), false);

                //訓練後に毎回更新を実行しなければ、ミニバッチとして更新できる
                nn.Update();
            }

            //訓練結果を表示
            Console.WriteLine("Test Start...");
            foreach (double[] val in trainData)
            {
                NdArray result = Trainer.Predict(nn, val);
                Console.WriteLine(val[0] + " xor " + val[1] + " = " + (result.Data[0] > 0.5?1:0) + " " + result);

            }
        }
    }
}