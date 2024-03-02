Imports System
Imports System.IO
Imports System.Diagnostics

Module Program

    Private Class Para
        Public Key As String
        Public Val As String
        Public Sub New(key As String, val As String)
            Me.Key = key
            Me.Val = val
        End Sub
    End Class

    Private Const EXE_DEF = "BLFlashCommand.exe"
    'Private Const EXE_DEF = "D:\Arduino Learning\AiPi-UNO-ET485\Tools\bouffalo_flash_cube_v1.0.4\BLFlashCommand.exe"

    Private Const TEMPLATE =
"[cfg]
# 0: no erase, 1:programmed section erase, 2: chip erase
erase = 1
# skip mode set first para is skip addr, second para is skip len, multi-segment region with ; separated
skip_mode = 0x0, 0x0
# 0: not use isp mode, #1: isp mode
boot2_isp_mode = 0

[boot2]
filedir = {{boot2}}
address = 0x000000

[partition]
filedir = {{partit}}
address = 0xE000

[FW]
filedir = {{project}}
address = 0x10000

[mfg]
filedir = {{mfg}}
address = 0x210000
"

    Private buildpath As String = Nothing, projectname As String = Nothing, EXE As String = EXE_DEF

    Sub Main(args As String())
        'buildpath = "D:\Arduino Learning\AiPi-UNO-ET485\Firmwares\flash_prog_cfg"
        'projectname = "WiFiClientConnect.ino"

        Console.WriteLine("Welcome to MyBLFlashCommand ^_^")
        Console.WriteLine("I is created for upload firmware with the Arduino IDE and BLFlashCommand V1.0.4")
        Console.WriteLine()

        ' CMD Arguments are contained in the args variable
        'Console.WriteLine("CommandLineArgs: {0}", String.Join(", ", args))

        ' Getting parameter
        Dim paras As New List(Of Para)
        For i As Integer = 0 To args.Length - 1
            Console.WriteLine("[{0}] {1}", i, args(i))
            Dim ks = args(i).Split("=")
            If ks.Length = 2 Then
                Dim k = ks(0)
                Dim v = ks(1)
                'Console.WriteLine("{0}:{1} ", k, v)
                If k.Equals("--buildpath") Then
                    buildpath = v
                ElseIf k.Equals("--projectname") Then
                    projectname = v
                ElseIf k.Equals("--exe") Then
                    EXE = v
                ElseIf k.Equals("--help") OrElse k.Equals("-h") Then
                    ShowHelp()
                Else
                    paras.Add(New Para(k, v))
                End If
            End If
        Next

        'Check data
        If paras.Count > 0 AndAlso (Not String.IsNullOrEmpty(buildpath)) AndAlso (Not String.IsNullOrEmpty(projectname)) Then
            Try
                'Create config file
                Dim path = SaveConfigFile()
                If String.IsNullOrEmpty(path) Then
                    Throw New Exception("No input file")
                End If
                'Add "..." to path
                If path.Contains(" "c) Then
                    path = """" & path & """"
                End If
                paras.Add(New Para("--config", path))
                'Run flash process
                RunFlash(paras)
            Catch ex As Exception
                ' Display the error message
                Console.WriteLine("Error: " & ex.Message)

                ' Exit the console application with an error code
                Environment.Exit(1)
            End Try
        Else
            'Show help contain
            Console.WriteLine("Error: No Parameter!")
            ShowHelp()
        End If
    End Sub

    Private Sub ShowHelp()
        Console.WriteLine(vbCrLf & "Command format:")
        Console.WriteLine("MyBLFlashCommand.exe --interface=uart --chipname=bl616 --port={com_port} --baudrate=2000000 --buildpath=""{build.path}"" --projectname={build.project_name} --exe={path of BLFlashCommand.exe}")
        Console.WriteLine()

        Console.WriteLine("[1] Command Example, when MyBLFlashCommand.exe is in the same folder as BLFlashCommand.exe")
        Console.WriteLine("MyBLFlashCommand.exe --interface=uart --chipname=bl616 --port=COM11 --baudrate=2000000 --buildpath=""C:\Users\User\AppData\Local\Temp\arduino\sketches\B723E196EE1AD2E04E7CD18725A76A3C"" --projectname=Blink.ino")
        Console.WriteLine()

        Console.WriteLine("[2] Command Example, when MyBLFlashCommand.exe is not in the same folder as BLFlashCommand.exe")
        Console.WriteLine("MyBLFlashCommand.exe --interface=uart --chipname=bl616 --port=COM11 --baudrate=2000000 --buildpath=""C:\Users\User\AppData\Local\Temp\arduino\sketches\B723E196EE1AD2E04E7CD18725A76A3C"" --projectname=Blink.ino --exe=""D:\Arduino Learning\AiPi-UNO-ET485\Tools\bouffalo_flash_cube_v1.0.4\BLFlashCommand.exe""")
        Console.WriteLine()

        Console.WriteLine("List of .bin file that required on {build.path} folder:")
        Console.WriteLine("- boot2_xxxx.bin")
        Console.WriteLine("- mfg_xxxx.bin")
        Console.WriteLine("- partition.bin")
        Console.WriteLine("- {build.project.name}.bin (Ex. Blink.ino.bin)")
        Console.WriteLine()

        'Auto exit
        Environment.Exit(0)
    End Sub

    Private Function SaveConfigFile() As String
        Dim p As String = ""
        Dim cf As String = TEMPLATE
        Dim fw As String = projectname + ".bin"
        ' Get a list of all files in the directory and its subdirectories
        Dim files As String() = Directory.GetFiles(buildpath, "*", SearchOption.AllDirectories)

        ' Output the list of files with their full paths
        Console.WriteLine("Getting file list...")
        Dim n As Integer = 0
        For Each file As String In files
            Dim fname = Path.GetFileName(file)
            Console.WriteLine(fname)
            If fname.StartsWith("boot2") Then
                cf = cf.Replace("{{boot2}}", fname)
                n += 1
            End If
            If fname.StartsWith("mfg") Then
                cf = cf.Replace("{{mfg}}", fname)
                n += 1
            End If
            If fname.StartsWith("partit") Then
                cf = cf.Replace("{{partit}}", fname)
                n += 1
            End If
            If fname.Equals(fw) Then
                cf = cf.Replace("{{project}}", fw)
                n += 1
            End If
        Next
        Console.WriteLine("Updated {0}", n)
        Console.WriteLine(cf)
        If n = 4 Then
            'Set file path
            p = buildpath + "/flash_prog_cfg.ini"
            'Save file
            File.WriteAllText(p, cf)
            Console.WriteLine("File saved: {0}" & Environment.NewLine, p)
        End If
        Return p
    End Function

    Private Sub RunFlash(paras As List(Of Para))
        Dim cmd As String = ""
        For Each x In paras
            cmd &= x.Key & " " & x.Val & " "
        Next
        Console.WriteLine(EXE & " " & cmd)

        ' Create a new process object
        Dim process As New Process()

        ' Specify the filename (executable) of the process to start
        process.StartInfo.FileName = EXE

        ' Specify the arguments for the process (e.g., "/C echo Hello, World!")
        process.StartInfo.Arguments = cmd

        ' Start the process
        process.Start()

        ' Wait for the process to exit
        process.WaitForExit()
    End Sub
End Module
