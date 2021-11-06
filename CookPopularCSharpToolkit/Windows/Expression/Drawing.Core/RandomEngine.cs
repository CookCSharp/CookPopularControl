using System;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：RandomEngine
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:56:43
 */
namespace CookPopularCSharpToolkit.Windows.Expression
{
    /// <summary>
    /// A random generator that supports uniform and Gaussian distributions.
    /// </summary>
    internal class RandomEngine
    {
        public RandomEngine(long seed)
        {
            this.Initialize(seed);
        }

        public double NextGaussian(double mean, double variance)
        {
            return this.Gaussian() * variance + mean;
        }

        public double NextUniform(double min, double max)
        {
            return this.Uniform() * (max - min) + min;
        }

        private void Initialize(long seed)
        {
            this.random = new Random((int)seed);
        }

        private double Uniform()
        {
            return this.random.NextDouble();
        }

        /// <summary>
        /// Generates a pair of independent, standard, normally distributed random numbers,
        /// zero expectation, unit variance, using polar form of the Box-Muller transformation.
        /// </summary>
        private double Gaussian()
        {
            if (this.anotherSample != null)
            {
                double value = this.anotherSample.Value;
                this.anotherSample = null;
                return value;
            }
            double num;
            double num2;
            double num3;
            do
            {
                num = 2.0 * this.Uniform() - 1.0;
                num2 = 2.0 * this.Uniform() - 1.0;
                num3 = num * num + num2 * num2;
            }
            while (num3 >= 1.0);
            double num4 = Math.Sqrt(-2.0 * Math.Log(num3) / num3);
            this.anotherSample = new double?(num * num4);
            return num2 * num4;
        }

        private Random random;

        private double? anotherSample;
    }
}
