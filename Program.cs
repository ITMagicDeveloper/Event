using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventDemo
{
    class Program
    {

        static void Main(string[] args)
        {
            //Heater heater = new Heater();
            //Alarm alarm = new Alarm();

            //heater.Boiled += alarm.MakeAlert;

            //heater.Boiled += (new Alarm()).MakeAlert;

            //heater.Boiled += new Heater.BoiledEventHandler(alarm.MakeAlert);

            //heater.Boiled += Display.ShowMsg;

            //heater.BoilWater();

            Test test = new Test();
            A a = new A();
            B b = new B()
            {
                Num = 100
            };
            C c = new C()
            {
                str = "CCC"
            };

            test.AddEvent(a.Back);

            test.Start(c);

            Console.Read();
        }
    }

    class Test
    {
        public delegate void BoiledEventHandler(object sender);

        public event BoiledEventHandler Boiled;

        public void AddEvent(BoiledEventHandler e)
        {
            Boiled += e;
        }

        public void Start(object obj)
        {
            Boiled?.Invoke(obj);
        }
    }

    class A
    {
        public void Back(object obj)
        {
            B b = obj as B;

            Console.WriteLine(b?.Num);
        }
    }

    class C
    {
        public string str;
    }


    class B
    {
        public int Num;
    }
    
    /// <summary>
    /// 热水器
    /// </summary>
    class Heater
    {
        private int temperature;

        public string Type = "RealFire 001";

        /// <summary>
        /// 添加型号作为演示
        /// </summary>
        public string Area = "China Xian";

        /// <summary>
        /// 声明委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void BoiledEventHandler(Object sender, BoiledEventArgs e);

        public event BoiledEventHandler Boiled;

        /// <summary>
        /// 定义BoiledEventArgs类，传递给Observer所感兴趣的信息
        /// </summary>
        public class BoiledEventArgs : EventArgs
        {
            public readonly int temperature;

            public BoiledEventArgs(int _temperature)
            {
                temperature = _temperature;
            }
        }

        /// <summary>
        /// 可以供继承自 Heater 的类重写，以便继承类拒绝其他对象对它的监视
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnBoiled(BoiledEventArgs e)
        {
            // 如果有对象注册
            Boiled?.Invoke(this, e);
            // 调用所有注册对象的方法
        }

        /// <summary>
        /// 烧水
        /// </summary>
        public void BoilWater()
        {
            for (int i = 0; i < 100; i++)
            {
                temperature = i;

                if (temperature > 95)
                {
                    //建立BoliedEventArgs 对象。
                    BoiledEventArgs e = new BoiledEventArgs(temperature);

                    // 调用 OnBolied方法
                    OnBoiled(e);
                }
            }
        }
    }

    class Alarm
    {
        public void MakeAlert(object sender, Heater.BoiledEventArgs e)
        {
            Heater heater = (Heater) sender;

            //访问 sender 中的公共字段
            Console.WriteLine("Alarm：{0} - {1}: ", heater.Area, heater.Type);
            Console.WriteLine("Alarm: 嘀嘀嘀，水已经 {0} 度了：", e.temperature);
            Console.WriteLine();
        }
    }

    class Display
    {
        public static void ShowMsg(Object sender, Heater.BoiledEventArgs e)
        {
            //静态方法
            Heater heater = (Heater)sender;

            Console.WriteLine("Display：{0} - {1}: ", heater.Area, heater.Type);

            Console.WriteLine("Display：水快烧开了，当前温度：{0}度。", e.temperature);

            Console.WriteLine();
        }
    }
}
