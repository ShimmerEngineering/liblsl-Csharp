This has been updated just to test the feasibility of using the Shimmer API. Test looks promising, but no validation was done.

Steps to run the example
1. First, follow steps in the readme [here](https://github.com/ShimmerEngineering/liblsl-Csharp/tree/shimmer_dev).
2. Make sure sensor Accel is enabled on your shimmer device (it can be configured using Consensys)
3. Update the btComport (can be find in Bluetooth Settings or using Consensys) accordingly when intializing ShimmerLogAndStreamSystemSerialPort in SendData, for example 

![](img/vs2019_lsl_btComport.png)

![](img/vs2019_lsl_codeComport.png)

4. Run SendData to create a stream outlet and make the streaming data available for other platforms
5. Run it as Release in Visual Studio as shown

![](img/vs2019_lsl_release.png)

OR 

Run the exe through command prompt from within same folder as the dll file as shown

![](img/vs2019_lsl_dir.png)

![](img/vs2019_lsl_cmd.png)

6. Make sure the stream type in both SendData and ReceiveData are the same
7. Run ReceiveData to retrieve the streaming data
