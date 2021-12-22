using System;
using System.Threading;
using LSL;
using ShimmerAPI;

namespace LSLExamples
{
    class SendData
    {
        ShimmerLogAndStreamSystemSerialPort shimmer;
        StreamOutlet outlet;
        public static void Main(string[] args)
        {
            SendData p = new SendData();
            p.start();
            
        }

        public void start()
        {
            // create stream info and outlet
            using StreamInfo info = new StreamInfo("SendData", "Accel", 3, 51.2, channel_format_t.cf_float32, "test");
            outlet = new StreamOutlet(info);
            
            /*
            while (!Console.KeyAvailable)
            {
                // generate random data and send it
                for (int k = 0; k < data.Length; k++)
                    data[k] = rnd.Next(-100, 100);
                outlet.push_sample(data);
                Thread.Sleep(10);
            }*/
            int enabledSensors = ((int)ShimmerBluetooth.SensorBitmapShimmer3.SENSOR_A_ACCEL); // this is to enable the two EXG Chips on the Shimmer3
            double samplingRate = 512;
            //byte[] defaultECGReg1 = new byte[10] { 0x00, 0xA0, 0x10, 0x40, 0x40, 0x2D, 0x00, 0x00, 0x02, 0x03 }; //see ShimmerBluetooth.SHIMMER3_DEFAULT_ECG_REG1
            //byte[] defaultECGReg2 = new byte[10] { 0x00, 0xA0, 0x10, 0x40, 0x47, 0x00, 0x00, 0x00, 0x02, 0x01 }; //see ShimmerBluetooth.SHIMMER3_DEFAULT_ECG_REG2
            byte[] defaultECGReg1 = ShimmerBluetooth.SHIMMER3_DEFAULT_ECG_REG1; //also see ShimmerBluetooth.SHIMMER3_DEFAULT_TEST_REG1 && ShimmerBluetooth.SHIMMER3_DEFAULT_EMG_REG1
            byte[] defaultECGReg2 = ShimmerBluetooth.SHIMMER3_DEFAULT_ECG_REG2; //also see ShimmerBluetooth.SHIMMER3_DEFAULT_TEST_REG2 && ShimmerBluetooth.SHIMMER3_DEFAULT_EMG_REG2

            shimmer = new ShimmerLogAndStreamSystemSerialPort("ShimmerID1", "COM5", 51.2, 0, 4, enabledSensors, false, false, false, 0, 0, defaultECGReg1, defaultECGReg2, false);
            shimmer.UICallback += this.HandleEvent;
            shimmer.Connect();
            System.Console.WriteLine("IN ABOUT 5 SECONDS STREAMING WILL START AFTER THE BEEP");
            Thread.Sleep(5000);
            System.Console.Beep();

            shimmer.StartStreaming();
        }

            public void HandleEvent(object sender, EventArgs args)
        {
            CustomEventArgs eventArgs = (CustomEventArgs)args;
            int indicator = eventArgs.getIndicator();

            switch (indicator)
            {
                case (int)ShimmerBluetooth.ShimmerIdentifier.MSG_IDENTIFIER_STATE_CHANGE:
                    System.Diagnostics.Debug.Write(((ShimmerBluetooth)sender).GetDeviceName() + " State = " + ((ShimmerBluetooth)sender).GetStateString() + System.Environment.NewLine);
                    int state = (int)eventArgs.getObject();
                    if (state == (int)ShimmerBluetooth.SHIMMER_STATE_CONNECTED)
                    {
                        System.Console.WriteLine("Connected");
                    }
                    else if (state == (int)ShimmerBluetooth.SHIMMER_STATE_CONNECTING)
                    {
                        System.Console.WriteLine("Connecting");
                    }
                    else if (state == (int)ShimmerBluetooth.SHIMMER_STATE_NONE)
                    {
                        System.Console.WriteLine("Disconnected");
                    }
                    else if (state == (int)ShimmerBluetooth.SHIMMER_STATE_STREAMING)
                    {
                        System.Console.WriteLine("Streaming");
                    }
                    break;
                case (int)ShimmerBluetooth.ShimmerIdentifier.MSG_IDENTIFIER_NOTIFICATION_MESSAGE:
                    break;
                case (int)ShimmerBluetooth.ShimmerIdentifier.MSG_IDENTIFIER_DATA_PACKET:
                    ObjectCluster objectCluster = (ObjectCluster)eventArgs.getObject();
                    SensorData data = objectCluster.GetData(Shimmer3Configuration.SignalNames.LOW_NOISE_ACCELEROMETER_X, "CAL");
                    if (data != null)
                        System.Console.Write(data.Data + ",");
                        float[] dataArray = new float[3];
                        dataArray[0] = (float)objectCluster.GetData(Shimmer3Configuration.SignalNames.LOW_NOISE_ACCELEROMETER_X, "CAL").Data;
                        dataArray[1] = (float)objectCluster.GetData(Shimmer3Configuration.SignalNames.LOW_NOISE_ACCELEROMETER_Y, "CAL").Data;
                        dataArray[2] = (float)objectCluster.GetData(Shimmer3Configuration.SignalNames.LOW_NOISE_ACCELEROMETER_Z, "CAL").Data;
                        outlet.push_sample(dataArray);
                    break;
            }
        }
    }
}
