using System;

namespace  PJW
{
	public static partial class Utility
    {
		/// <summary>
		/// 随机相关
		/// </summary>
		public static class Random 
		{
			private static System.Random _Random=new System.Random((int)DateTime.Now.Ticks);
			
			/// <summary>
			/// 设置随机数种子
			/// </summary>
			/// <param name="seed">随机数种子</param>
			public static void SetSeed(int seed)
			{
				_Random=new System.Random(seed);
			}

			/// <summary>
			/// 返回非负随机数
			/// </summary>
			/// <returns>返回大于等于0且小于System.Int32.MaxValue的整数</returns>
			public static int GetRandom()
			{
				return _Random.Next();
			}

			/// <summary>
			/// 返回一个小于指定最大值的非负整数值
			/// </summary>
			/// <param name="maxValue">随机数的上限(随机数取不到该上限值)</param>
			/// <returns>返回大于等于0且小于maxValue的整数</returns>
			public static int GetRandom(int maxValue)
			{
				return _Random.Next(maxValue);
			}

			/// <summary>
			/// 返回一个大于指定最小值且小于最大值的非负整数值
			/// </summary>
			/// <param name="minValue">随机数的下限（随机数可以去到该下限值）</param>
			/// <param name="maxValue">随机数的上限(随机数取不到该上限值)</param>
			/// <returns>返回一个大于指定最小值且小于最大值的非负整数值</returns>
			public static int GetRandom(int minValue,int maxValue)
			{
				return _Random.Next(minValue,maxValue);
			}

			/// <summary>
			/// 返回一个0到1之间的随机数
			/// </summary>
			/// <returns>0到1之间的双精度浮点数</returns>
			public static double GetRandomDouble()
			{
				return _Random.NextDouble();
			}

			/// <summary>
			/// 用随机数填充指定字节数组的元素
			/// </summary>
			/// <param name="buffer">指定字节数组</param>
			public static void GetRandomBytes(byte[] buffer)
			{
				_Random.NextBytes(buffer);
			}
		}
	}
}