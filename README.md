# Simple CanSat Log File Decrpytor
This log file decryptor is designed to decrypt and generate readable log files from OrbiSat Oeiras CanSat offline logs.
Not only it removes the COBS encoding, but also formats the data for each DeviceId corresponding to its type.

## Usage
To use the program is very simple, just run the script with the log file as an argument:
```bash
dotnet run <log_file>
```
