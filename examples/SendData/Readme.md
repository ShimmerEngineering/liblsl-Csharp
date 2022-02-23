This has been updated just to test the feasibility of using the Shimmer API. Test looks promising, but no validation was done.

Steps to run the example
1. include lsl.dll into the liblsl project's root directory
2. right click lsl.dll file, select properties, update build action to content, copy to output directory to copy if newer
3. Make sure sensor Accel is enabled on your shimmer device (it can be enabled using Consensys)
4. Update the btComport used when intialize ShimmerLogAndStreamSystemSerialPort in SendData (You can find the Comport information in device manager or using Shimmer 9DoF Calibration)
5. Run SendData to create a stream outlet and make the streaming data available for other platforms
6. Make sure the stream type in both SendData and ReceiveData are the same
7. Run ReceiveData to retrieve the streaming data
