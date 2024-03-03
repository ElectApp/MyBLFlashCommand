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

## Editing and installation for Arduino IDE as follows.
*This must be done after installing the [BouffaloLab board](https://github.com/bouffalolab/arduino-bouffalo) on the Arduino IDE.
1. Download my_bflb_flash_tools_v1.0.4.zip from the lastest release version on Github, extract the files and copy the folder to C:\Users\{username}\AppData\Local\Arduino15\packages\bouffalolab\tools\bflb_flash_tools
2. Go to the folder as in step 1 (bflb_flash_tools)
2.1 Change the folder name from 1.0.7 to 1.0.7_master
2.2 Change the phone name from my_bouffalo_flash_cube_v1.0.4 is 1.0.7
3. Copy the platform.txt file and rename it platform.txt.master from C:\Users\{username}\AppData\Local\Arduino15\packages\bouffalolab\hardware\bouffalolab\1.0.5
4. Edit the platform.txt file as follows.
```sh
#------------------------------------------------- -------
## BouffaloLab Flash tools
#------------------------------------------------- -------

tools.bflb_flash_tools.path={runtime.tools.bflb_flash_tools.path}
tools.bflb_flash_tools.cmd=MyBLFlashCommand
tools.bflb_flash_tools.cmd.windows=MyBLFlashCommand.exe
tools.bflb_flash_tools.cmd.linux=BLFlashCommand-ubuntu
tools.bflb_flash_tools.cmd.macosx=BLFlashCommand-macos
tools.bflb_flash_tools.upload.params.verbose=
tools.bflb_flash_tools.upload.params.quiet=
tools.bflb_flash_tools.upload.protocol=uart

tools.bflb_flash_tools.upload.pattern="{path}/{cmd}" --interface={upload.protocol} --chipname=bl616 --port={serial.port} --baudrate={upload.speed} --buildpath={build.path} --projectname={build.project_name}
```
5. Close and open the Arduino IDE program.
6. Enter boot mode on the board and try uploading the firmware>> The output message must be similar to using MyBLFlashCommand.exe with command line