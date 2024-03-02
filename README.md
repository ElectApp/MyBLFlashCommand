# Welcome to MyBLFlashCommand ^_^
I is created for upload firmware with the Arduino IDE and BLFlashCommand V1.0.4

## Command format:
```sh
MyBLFlashCommand.exe --interface=uart --chipname=bl616 --port={com_port} --baudrate=2000000 --buildpath="{build.path}" --projectname={build.project_name} --exe={path of BLFlashCommand.exe}
```

## Command Example
1. MyBLFlashCommand.exe is in the same folder as BLFlashCommand.exe
```sh
MyBLFlashCommand.exe --interface=uart --chipname=bl616 --port=COM11 --baudrate=2000000 --buildpath="C:\Users\User\AppData\Local\Temp\arduino\sketches\B723E196EE1AD2E04E7CD18725A76A3C" --projectname=Blink.ino
```
2. MyBLFlashCommand.exe is not in the same folder as BLFlashCommand.exe
```sh
MyBLFlashCommand.exe --interface=uart --chipname=bl616 --port=COM11 --baudrate=2000000 --buildpath="C:\Users\User\AppData\Local\Temp\arduino\sketches\B723E196EE1AD2E04E7CD18725A76A3C" --projectname=Blink.ino --exe="D:\Arduino Learning\AiPi-UNO-ET485\Tools\bflb_flash_tools_v1.0.4\BLFlashCommand.exe"
```

## List of .bin file that required on {build.path} folder:
- boot2_xxxx.bin
- mfg_xxxx.bin
- partition.bin
- {build.project.name}.bin (Ex. Blink.ino.bin)