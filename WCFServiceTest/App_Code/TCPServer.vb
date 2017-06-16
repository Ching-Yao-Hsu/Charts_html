Imports System.Net.Sockets
Imports System.Threading
Imports System.IO
Imports System.Text
Imports System.Net.Mail

Public Class TCPServer
    Implements ITCPService
#Region "內部使用變數"
    Dim ListenSocket As TcpListener
    Dim ECO_Connect() As TcpClient
    ''' <summary>
    ''' 接聽處理緒
    ''' </summary>
    Dim ListenThread As New Thread(AddressOf Listening)
    ''' <summary>
    ''' 連線處理緒
    ''' </summary>
    Dim GetDataThread() As ThreadGetData
    Structure ThreadGetData
        Dim [Thread] As Thread
        Dim RspBytes() As Byte
    End Structure
    ''' <summary>
    ''' 宣告結構化變數，將ECO5及電表相關資料放入此變數
    ''' </summary>
    ''' <remarks></remarks>
    Structure ECO_Data
        Dim Account As String
        Dim Password As String
        Dim Enabled As Boolean
        Dim ECO_Type As String
        Dim MeterData As Meter_Data
    End Structure
    Structure Meter_Data
        Dim MeterID() As Byte
        'Dim DrawNr() As String
        'Dim InstallPosition() As String
        Dim Enabled() As Boolean
    End Structure
    
#End Region

#Region "屬性"
    ''' <summary>
    ''' 自動接聽的通訊埠號
    ''' </summary>
    Public PortNr As Int32 = 8500                  '宣告接收的連接埠，內定值=8500
    ''' <summary>
    ''' 自動校時功能
    ''' </summary>
    Public AutoTimeAdjust As Boolean = True         '自動校時
    ''' <summary>
    ''' 指定每日自動校時時間
    ''' </summary>
    Public AutoTimePoint As Date = #3:00:00 AM#     '自動校正時間，內定值為每日清晨03:00
    ''' <summary>
    ''' 允許資料格式錯誤的次數，超過設定次數時，本連線會中斷。
    ''' 當輸入 -1 時，不判斷資料格式錯誤
    ''' </summary>
    Public AllowErrCount As Integer = 5             '允許接收錯誤次數
    ''' <summary>
    ''' 允許電表通訊錯誤次數
    ''' </summary>
    Public AllowMeterErrCount() As UInt16 = {0, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10}
    ''' <summary>
    ''' 等待資料讀取的時間
    ''' </summary>
    Public ReadTimeOut As Integer = 10000           '等待讀取資料的時間，預設值為 10000ms(10sec)
    ''' <summary>
    ''' 取得指定連線的連線位址及通訊埠
    ''' </summary>
    ''' 遠端連線IP，唯讀
    ReadOnly Property RemoteIP(ByVal Index As Integer) As String
        Get
            Return GetRemoteIP(Index)
        End Get
    End Property
    ''' <summary>
    ''' 是否觸發到達記錄間隔時間事件
    ''' </summary>
    ''' <remarks></remarks>
    Public RecEventEnabled As Boolean = True
    ''' <summary>
    ''' 觸發資料記錄事件的間隔時間
    ''' 單位為分鐘
    ''' </summary>
    Public RecInterval As Single = 1              '引發紀錄事件的間隔時間
    ''' <summary>
    ''' 版權所有
    ''' </summary>
    'Public ReadOnly CopyRight As String = My.Application.Info.Copyright
    ''' <summary>
    ''' 檔案版本
    ''' </summary>
    'Public ReadOnly Version As String = My.Application.Info.Version.ToString
#End Region

#Region "事件"
    ''' <summary>
    ''' 新連線事件
    ''' </summary>
    Event NewConnect(ByVal Index As Integer, ByVal RemoteIP As String, ByVal PortNr As Integer)
    ''' <summary>
    ''' 連線中斷事件
    ''' </summary>
    ''' <param name="Index"></param>
    ''' <param name="RemoteIP"></param>
    ''' <param name="PortNr"></param>
    ''' <param name="ex"></param>
    ''' <remarks></remarks>
    Event ConnectBroken(ByVal Index As Integer, ByVal RemoteIP As String, ByVal PortNr As Integer, ByVal ex As String)
    ''' <summary>
    ''' 錯誤事件
    ''' </summary>
    ''' <param name="Index">連線索引值</param>
    ''' <param name="ex"></param>
    ''' <param name="RemoteIP">連線IP</param>
    ''' <param name="PortNr">連線通訊埠</param>
    ''' <remarks></remarks>
    Event OnError(ByVal Index As Integer, ByVal ex As Exception, ByVal RemoteIP As String, ByVal PortNr As Integer)
    ''' <summary>
    ''' 原始資料事件
    ''' </summary>
    ''' 接收資料事件
    Event GetData(ByVal Index As Integer, ByVal ByteLength As Integer, ByVal bytes() As Byte)
    ''' <summary>
    ''' 轉譯成ECO5資料事件
    ''' </summary>
    ''' 接收並將資料解析
    ''' Index:本連線的索引值
    ''' Legal:本次資料格式是否合法
    ''' Contents:接收的資料內容
    Event TransToECOsData(ByVal Index As Integer, ByVal Legal As Boolean, ByVal Contents As Content)
    ''' <summary>
    ''' 到達記錄間隔時間
    ''' </summary>
    ''' <param name="Index">指定的連線索引值</param>
    ''' <param name="Contents">ECO5的資料內容</param>
    Event RecTimeUp(ByVal Index As Integer, ByVal Contents As Content)
    ''' <summary>
    ''' 超約事件
    ''' </summary>
    ''' <param name="Index">連線索引值</param>
    ''' <param name="MeterID">電表站號</param>
    Event OverDemand(ByVal Index As Integer, ByVal Account As String, ByVal MeterID As Byte, ByVal RecDate As String, ByVal RecTime As String)
    ''' <summary>
    ''' 電流異常事件
    ''' </summary>
    ''' <param name="Index">連線索引值</param>
    ''' <param name="MeterID">電表站號</param>
    Event CurrentAlarm(ByVal Index As Integer, ByVal Account As String, ByVal MeterID As Byte, ByVal RecDate As String, ByVal RecTime As String)
    ''' <summary>
    ''' 電壓異常事件
    ''' </summary>
    ''' <param name="Index">連線索引值</param>
    ''' <param name="MeterID">電表站號</param>
    Event VotageAlarm(ByVal Index As Integer, ByVal Account As String, ByVal MeterID As Byte, ByVal RecDate As String, ByVal RecTime As String)
    ''' <summary>
    ''' 超過指定用電量事件
    ''' </summary>
    ''' <param name="Index">連線索引值</param>
    ''' <param name="MeterID">電表站號</param>
    Event EnergyOverUse(ByVal Index As Integer, ByVal Account As String, ByVal MeterID As Byte, ByVal RecDate As String, ByVal RecTime As String)
    ''' <summary>
    ''' 電表通訊異常事件
    ''' </summary>
    ''' <param name="Index">連線索引值</param>
    ''' <param name="MeterID">電表站號</param>
    Event CommError(ByVal Index As Integer, ByVal Account As String, ByVal MeterID As Byte, ByVal RecDate As String, ByVal RecTime As String)
#End Region

#Region "方法"
    ''' <summary>
    ''' 開始自動接聽連線
    ''' </summary>
    Sub Start()
        ListenSocket = New TcpListener(Net.IPAddress.Any, PortNr)
        ListenThread.Start()
    End Sub

    ''' <summary>
    ''' 設定時間至ECOs
    ''' </summary>
    ''' 設定ECOs時間
    ''' <param name="Index">指定的連線索引值</param>
    ''' <param name="Time">選擇性引數，不指定時，以系統時間為指定時間</param>
    Function SetECOsTime(ByVal Index As Integer, Optional ByVal Time As Date = Nothing) As Boolean
        Dim bytes() As Byte = {1, 16, 31, 129, 0, 4, 8, 0, 0, 0, 0, 0, 0, 0, 0}
        If Time = Nothing Then Time = Now
        bytes(7) = Right(Time.Year.ToString, 2)
        bytes(8) = Time.Month
        bytes(9) = Time.Day
        bytes(10) = Time.Hour
        bytes(11) = Time.Minute
        bytes(12) = Time.Second
        bytes(13) = Time.DayOfWeek
        Return Write(Index, bytes)
    End Function

    ''' <summary>
    ''' 停止目前連線的主動上傳功能
    ''' 注意：停止主動上傳功能後，無法再透過通訊命令啟動！
    ''' </summary>
    ''' <param name="Index">指定的連線索引值</param>
    Function StopUpLoad(ByVal Index As Integer) As Boolean
        Dim bytes() As Byte = {1, 5, &HD, &H33, &HFF, 0}
        Return WriteToConnect(Index, bytes)
    End Function

    ''' <summary>
    ''' 停止自動接聽，並結束所有連線
    ''' </summary>
    ''' 結束 TCPServer
    Sub [Stop]()
        '結束TCPListener的聆聽
        ListenSocket.Stop()
        '結束聆聽執行緒
        ListenThread.Abort()
        '關閉所有 TcpClient
        For Each a As TcpClient In ECO_Connect
            If Not IsNothing(a) Then
                a.Close()
            End If
        Next
    End Sub

    ''' <summary>
    ''' 中斷指定連線
    ''' </summary>
    ''' 中斷指定的連線
    ''' <param name="Index">指定的連線索引值</param>
    Function DisConnect(ByVal Index As Integer) As Boolean
        Try
            If Not IsNothing(ECO_Connect(Index)) Then
                ECO_Connect(Index).Close()
                Return True
            End If
            Return False
        Catch ex As Exception
            RaiseEvent OnError(Index, ex, Nothing, Nothing)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 寫資料至指定連線
    ''' </summary>
    ''' 寫資料至指定的連線
    ''' <param name="Index">指定的連線索引值</param>
    ''' <param name="bytes">寫入的資料陣列</param>
    Function Write(ByVal Index As Integer, ByVal bytes() As Byte) As Boolean
        Try
            If Not IsNothing(ECO_Connect(Index)) Then
                GetDataThread(Index).RspBytes = bytes
                Return True
            End If
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function
#End Region

#Region "內部程序"
    ''' <summary>
    ''' 聆聽並等待連線
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Listening()
        Dim Index As Integer
        ListenSocket.Start()
        ReDim GetDataThread(Index)
        ReDim ECO_Connect(Index)
        While True
            Dim a As Integer = 0
            While a <= Index
                If IsNothing(GetDataThread(a).[Thread]) Then
                    ListenProcess(a)
                    a = 0
                ElseIf Not GetDataThread(a).[Thread].IsAlive Then
                    ListenProcess(a)
                    a = 0
                Else
                    a += 1
                End If
            End While
            Index += 1
            ReDim Preserve ECO_Connect(Index)
            ReDim Preserve GetDataThread(Index)
        End While
    End Sub

    ''' <summary>
    ''' 聆聽子程序，等待新的連線
    ''' 當接聽新的連線後，產生一新的執行緒:GetDataThread，並給定一用以判別之Index值
    ''' </summary>
    Private Sub ListenProcess(ByVal Index As Integer)
        Try
            ECO_Connect(Index) = ListenSocket.AcceptTcpClient
            Dim IPTmp() As String = GetRemoteIP(Index).Split(":")
            '觸發新連線事件
            RaiseEvent NewConnect(Index, IPTmp(0), Int(IPTmp(1)))
            GetDataThread(Index).[Thread] = New Thread(AddressOf GetDataFromConnect)
            GetDataThread(Index).[Thread].Start(Index)
        Catch ex As SocketException
            RaiseEvent OnError(Index, ex, Nothing, Nothing)
        End Try
    End Sub

    '取得指定連線的IP
    Private Function GetRemoteIP(ByVal Index As Integer) As String
        Try
            Return ECO_Connect(Index).Client.RemoteEndPoint.ToString
        Catch ex As Exception
            RaiseEvent OnError(Index, ex, Nothing, Nothing)
            Return Nothing
        End Try
    End Function

    '資料接收讀取
    Private Sub GetDataFromConnect(ByVal Index As Integer)
        Dim IPTmp() As String = GetRemoteIP(Index).Split(":")
        Dim stream As NetworkStream = ECO_Connect(Index).GetStream
        stream.ReadTimeout = ReadTimeOut
        Dim ErrCount As Integer = 0
        Dim TimeBeenSet As Boolean = False
        Dim RecTimeStamp As Date = Now
        Dim RecBit(9) As Boolean
        Dim RecTimeOn As Boolean = False

        Dim MeterErrCount(10) As UInt16
        '各用電區超過指定用電量狀態
        Dim Energy_OverUse(10) As Boolean
        '各電表通訊錯誤狀態
        Dim Comm_Error(10) As Boolean
        '各電表電壓異常狀態
        Dim Votage_Error(10) As Boolean
        '各電表電流異常狀態
        Dim Current_Error(10) As Boolean
        '各電表需量異常狀態
        Dim Demand_Error(10) As Boolean
        Try
            While True
                Dim bytes(1023) As Byte
                Dim i As Int32
                i = stream.Read(bytes, 0, bytes.Length)
                '連線已中斷
                If i = 0 Then
                    ECO_Connect(Index).Close()
                    RaiseEvent ConnectBroken(Index, IPTmp(0), Int(IPTmp(1)), "遠端連線已中斷")
                    Exit While
                End If
                ReDim Preserve bytes(i - 1)
                RaiseEvent GetData(Index, i, bytes)

                Dim TranslateData As Content = TransDataFormatToECO5(Index, bytes)
                '判斷資料格式錯誤次數及處理方式
                If TranslateData.Result Then
                    ErrCount = 0
                    '電表事件處理
                    MeterEvent(Index, TranslateData, Comm_Error, MeterErrCount, Energy_OverUse, Votage_Error, Current_Error, Demand_Error)
                    '觸發紀錄事件
                    If RecEventEnabled Then
                        RecordEventTreat(Index, TranslateData, RecTimeStamp, RecTimeOn, RecBit)
                    End If
                Else
                    If AllowErrCount <> -1 Then ErrCount += 1
                End If
                If ErrCount >= AllowErrCount And AllowErrCount <> -1 Then
                    ECO_Connect(Index).Close()
                    RaiseEvent ConnectBroken(Index, IPTmp(0), Int(IPTmp(1)), "資料格式錯誤超過允許次數： " & AllowErrCount & "次,中斷連線。")
                    Exit While
                End If
                '判斷是否自動校時
                If AutoTimeAdjust Then
                    If TimeOfDay.Subtract(AutoTimePoint).TotalMinutes >= 0 Then
                        If Not TimeBeenSet Then
                            TimeBeenSet = SetECOsTime(Index)
                        End If
                    Else
                        If TimeOfDay.Subtract(AutoTimePoint).TotalMinutes < 0 Then
                            TimeBeenSet = False
                        End If
                    End If
                End If
                Dim Rspbytes() As Byte
                With GetDataThread(Index)
                    If IsNothing(.RspBytes) Then
                        'Rspbytes = {1, 3, 4, 1, 1, 1, 1}
                        ReDim Rspbytes(6)
                        Rspbytes(0) = 1
                        Rspbytes(1) = 3
                        Rspbytes(2) = 4
                        Rspbytes(3) = 1
                        Rspbytes(4) = 1
                        Rspbytes(5) = 1
                        Rspbytes(6) = 1
                    Else
                        Rspbytes = .RspBytes
                        .RspBytes = Nothing
                    End If
                End With
                WriteToConnect(Index, Rspbytes)
            End While
        Catch ex As IOException
            If TypeName(ex.InnerException) = "SocketException" Then
                Dim TmpEx As SocketException = ex.InnerException
                Select Case TmpEx.ErrorCode
                    Case 10004  '使用者中斷連線
                        RaiseEvent ConnectBroken(Index, IPTmp(0), Int(IPTmp(1)), "使用者中斷連線")
                    Case 10060  '超過讀取等待時間
                        ECO_Connect(Index).Close()
                        RaiseEvent ConnectBroken(Index, IPTmp(0), Int(IPTmp(1)), "超過讀取等待時間")
                End Select
            End If
        Catch ex As ObjectDisposedException
            'TCPClient被關閉時，進行NetworkStream的相關動作時發生
            '接收資料時，使用者同步中斷連線時發生
            RaiseEvent ConnectBroken(Index, IPTmp(0), Int(IPTmp(1)), "使用者中斷連線")
        Finally
            stream.Close()
            ECO_Connect(Index) = Nothing
        End Try
    End Sub

    '建構 GetTransfer 事件中 Contents 變數的資料結構
    Structure Content
        ''' <summary>
        ''' ECO5資料格式判斷結果
        ''' </summary>
        Dim Result As Boolean           '結果
        ''' <summary>
        ''' 帳號
        ''' </summary>
        Dim Account As String           '帳號
        ''' <summary>
        ''' 密碼
        ''' </summary>
        Dim Password As String          '密碼
        ''' <summary>
        ''' 電表站號
        ''' </summary>
        Dim MeterID As Byte             '電表ID
        ''' <summary>
        ''' 電表是否作用
        ''' </summary>
        Dim MeterEnabled As Boolean     '電表是否使用
        ''' <summary>
        ''' 記錄日期
        ''' </summary>
        Dim RecDate As String           '本筆資料日期
        ''' <summary>
        ''' 紀錄時間
        ''' </summary>
        Dim RecTime As String           '本筆資料時間
        ''' <summary>
        ''' 記錄狀態(原始值，16進制)
        ''' </summary>
        Dim RecState As String          '本筆資料狀態
        ''' <summary>
        ''' 電力數值
        ''' </summary>
        Dim Value() As Single           '值
        ''' <summary>
        ''' 紀錄是否合法
        ''' 當電表通訊不正確或其它原因，本項為False
        ''' </summary>
        Dim RecLegal As Boolean         '本筆資料異常/正確
        ''' <summary>
        ''' 時間電價模式
        ''' </summary>
        Dim TimeMode As Byte            '電價模式
        ''' <summary>
        ''' 時間電價時段
        ''' </summary>
        Dim TimePeriod As Byte          '目前時段:0:離峰 1:尖峰 2:半尖峰 3:週六半尖峰
        ''' <summary>
        ''' 是否啟用需量功能
        ''' </summary>
        Dim DemandEnabled As Boolean    '啟動需量
        ''' <summary>
        ''' KWH值是否正確
        ''' 當DemandEnabled=True時，本項才有意義
        ''' </summary>
        Dim KWH_State As Boolean        'KWH狀態
        ''' <summary>
        ''' 超過需量
        ''' </summary>
        Dim HiDemand As Boolean         '超約
        ''' <summary>
        ''' 低於需量下限
        ''' </summary>
        Dim LoDemand As Boolean         '低負載
        ''' <summary>
        ''' 過電流警報
        ''' </summary>
        Dim CurrentError As Boolean     '電流異常
        ''' <summary>
        ''' 過電壓警報
        ''' </summary>
        Dim VotageError As Boolean      '電壓異常
        ''' <summary>
        ''' 超過指定用電量
        ''' </summary>
        Dim EnergyOverUse As Boolean    '電量過度使用
    End Structure

    ''' <param name="Bytes">原始資料</param>
    ''' 將接收到的資料進行轉譯
    Private Function TransDataFormatToECO5(ByVal Index As Integer, ByVal Bytes() As Byte) As Content
        Dim TransferData As Content = Nothing
        If Bytes.Length <> 27 And Bytes.Length <> 147 Then
            RaiseEvent TransToECOsData(Index, False, Nothing)
            Return TransferData
        End If

        If Bytes(2) <> 24 And Bytes(2) <> 144 Then
            RaiseEvent TransToECOsData(Index, False, Nothing)
            Return TransferData
        End If

        Try
            With TransferData
                .Result = True
                .Account = Encoding.ASCII.GetString(Bytes, 3, 15)
                .Password = Encoding.ASCII.GetString(Bytes, 18, 6)
                .MeterID = Bytes(0)
                .MeterEnabled = Bytes(24)
                Select Case Bytes(2)
                    Case 24

                    Case 144
                        .RecDate = "20" & Hex(Bytes(117)).PadLeft(2, "0") & "/" & _
                                Hex(Bytes(118)).PadLeft(2, "0") & "/" & Hex(Bytes(143)).PadLeft(2, "0")
                        .RecTime = Hex(Bytes(144)).PadLeft(2, "0") & ":" & _
                                Hex(Bytes(145)).PadLeft(2, "0") & ":" & Hex(Bytes(146)).PadLeft(2, "0")
                        .RecState = Hex(Bytes(115)).PadLeft(2, "0") & Hex(Bytes(116)).PadLeft(2, "0")
                        .RecLegal = Mid(Convert.ToString(Bytes(115), 2).PadLeft(8, "0"), 1, 1)

                        .KWH_State = Mid(Convert.ToString(Bytes(115), 2).PadLeft(8, "0"), 2, 1)
                        .EnergyOverUse = Mid(Convert.ToString(Bytes(115), 2).PadLeft(8, "0"), 7, 1)

                        .VotageError = Mid(Convert.ToString(Bytes(115), 2).PadLeft(8, "0"), 8, 1)

                        Dim StateTmp As String = Convert.ToString(Bytes(116), 2).PadLeft(8, "0")
                        .CurrentError = Mid(StateTmp, 1, 1)

                        '本行有問題，實際控制器並未定義此接點
                        '.DemandEnabled = Mid(StateTmp, 1, 1)
                        .HiDemand = Mid(StateTmp, 8, 1)

                        .LoDemand = Mid(StateTmp, 7, 1)
                        .TimeMode = Mid(StateTmp, 6, 1)
                        If Mid(StateTmp, 5, 1) Then
                            .TimePeriod = 1
                        ElseIf Mid(StateTmp, 4, 1) Then
                            .TimePeriod = 2
                        ElseIf Mid(StateTmp, 3, 1) Then
                            .TimePeriod = 3
                        Else
                            .TimePeriod = 0
                        End If
                        ReDim .Value(24)
                        '電流
                        .Value(0) = BitConverter.ToInt32(Bytes, 27) / 1000
                        .Value(1) = BitConverter.ToInt32(Bytes, 31) / 1000
                        .Value(2) = BitConverter.ToInt32(Bytes, 35) / 1000
                        .Value(3) = BitConverter.ToInt32(Bytes, 67) / 1000
                        '電壓
                        .Value(4) = BitConverter.ToInt32(Bytes, 39) / 100
                        .Value(5) = BitConverter.ToInt32(Bytes, 43) / 100
                        .Value(6) = BitConverter.ToInt32(Bytes, 47) / 100
                        .Value(7) = BitConverter.ToInt32(Bytes, 71) / 100
                        'KW
                        .Value(8) = BitConverter.ToInt32(Bytes, 51) / 100
                        'KVAR
                        .Value(9) = BitConverter.ToInt32(Bytes, 55) / 100
                        'KVA
                        .Value(10) = BitConverter.ToInt32(Bytes, 59) / 100
                        'PF
                        .Value(11) = BitConverter.ToInt32(Bytes, 75) / 1000
                        'KWH
                        .Value(12) = BitConverter.ToInt32(Bytes, 63)
                        'Mode1~Mode4
                        .Value(13) = BitConverter.ToInt32(Bytes, 79) / 10
                        .Value(14) = BitConverter.ToInt32(Bytes, 83) / 10
                        .Value(15) = BitConverter.ToInt32(Bytes, 87) / 10
                        .Value(16) = BitConverter.ToInt32(Bytes, 91) / 10
                        'Max
                        .Value(17) = BitConverter.ToInt32(Bytes, 103)
                        .Value(18) = BitConverter.ToInt32(Bytes, 119)
                        .Value(19) = BitConverter.ToInt32(Bytes, 123)
                        .Value(20) = BitConverter.ToInt32(Bytes, 127)
                        'KWH Period
                        .Value(21) = BitConverter.ToInt32(Bytes, 95)
                        .Value(22) = BitConverter.ToInt32(Bytes, 107)
                        .Value(23) = BitConverter.ToInt32(Bytes, 111)
                        .Value(24) = BitConverter.ToInt32(Bytes, 99)
                End Select
            End With

            '觸發事件
            RaiseEvent TransToECOsData(Index, True, TransferData)
            Return TransferData
        Catch ex As Exception
            RaiseEvent TransToECOsData(Index, False, Nothing)
            Return TransferData
        End Try
    End Function

    '電表事件處理
    Private Sub MeterEvent(ByVal Index As Integer, ByVal TranslateData As Content, ByRef Comm_Error() As Boolean, ByRef MeterErrCount() As UInt16, _
                           ByRef Energy_OverUse() As Boolean, ByRef Votage_Error() As Boolean, ByRef Current_Error() As Boolean, _
                           ByRef Demand_Error() As Boolean)
        With TranslateData
            If .MeterEnabled Then
                '觸發電表通訊錯誤事件
                If .RecLegal Then
                    Comm_Error(.MeterID) = False
                    MeterErrCount(.MeterID) = 0
                Else
                    If MeterErrCount(.MeterID) >= AllowMeterErrCount(.MeterID) Then
                        MeterErrCount(.MeterID) = AllowMeterErrCount(.MeterID)
                        If Not Comm_Error(.MeterID) Then
                            RaiseEvent CommError(Index, .Account, .MeterID, .RecDate, .RecTime)
                            Comm_Error(.MeterID) = True
                        End If
                    Else
                        MeterErrCount(.MeterID) += 1
                    End If
                End If
                '觸發超過指定用電量事件
                If .EnergyOverUse Then
                    If Not Energy_OverUse(.MeterID) Then
                        RaiseEvent EnergyOverUse(Index, .Account, .MeterID, .RecDate, .RecTime)
                        Energy_OverUse(.MeterID) = True
                    End If
                Else
                    Energy_OverUse(.MeterID) = False
                End If
                '觸發電壓異常事件
                If .VotageError Then
                    If Not Votage_Error(.MeterID) Then
                        RaiseEvent VotageAlarm(Index, .Account, .MeterID, .RecDate, .RecTime)
                        Votage_Error(.MeterID) = True
                    End If
                Else
                    Votage_Error(.MeterID) = False
                End If
                '觸發電流異常事件
                If .CurrentError Then
                    If Not Current_Error(.MeterID) Then
                        RaiseEvent CurrentAlarm(Index, .Account, .MeterID, .RecDate, .RecTime)
                        Current_Error(.MeterID) = True
                    End If
                Else
                    Current_Error(.MeterID) = False
                End If
                '觸發超約事件
                If .HiDemand Then
                    If Not Demand_Error(.MeterID) Then
                        RaiseEvent OverDemand(Index, .Account, .MeterID, .RecDate, .RecTime)
                        Demand_Error(.MeterID) = True
                    End If
                Else
                    Demand_Error(.MeterID) = False
                End If
            End If
        End With
    End Sub

    ''' <summary>
    ''' 觸發紀錄事件的處理程序
    ''' </summary>
    ''' <param name="Index">連線索引值</param>
    ''' <param name="TranslateData">翻譯的內容</param>
    ''' <param name="RecTimeStamp">紀錄時間標記</param>
    ''' <param name="RecTimeOn">已到紀錄間隔時間</param>
    ''' <param name="RecBit"></param>
    ''' <remarks></remarks>
    Private Sub RecordEventTreat(ByVal Index As Integer, ByVal TranslateData As Content, ByRef RecTimeStamp As Date, ByRef RecTimeOn As Boolean, ByRef RecBit() As Boolean)
        If Now.Subtract(RecTimeStamp).TotalMinutes >= RecInterval Or RecTimeOn Then
            If Not RecTimeOn Then
                RecTimeStamp = RecTimeStamp.AddMinutes(RecInterval)
                RecTimeOn = True
            End If
            If TranslateData.MeterEnabled Then
                '電表啟用
                If RecBit(TranslateData.MeterID - 1) = False Then
                    RecBit(TranslateData.MeterID - 1) = True
                    RaiseEvent RecTimeUp(Index, TranslateData)
                End If
            Else
                '電表未啟用
                RecBit(TranslateData.MeterID - 1) = True
            End If
            '檢查是否已觸發所有記錄事件
            If RecBit(0) And RecBit(1) And RecBit(2) And RecBit(3) And RecBit(4) And RecBit(5) And RecBit(6) And RecBit(7) And RecBit(8) And RecBit(9) Then
                RecTimeOn = False
                For b = 0 To 9
                    RecBit(b) = False
                Next
            End If
        End If
    End Sub

    ''' <summary>
    ''' 將位元陣列資料寫至指定連線
    ''' </summary>
    Private Function WriteToConnect(ByVal Index As Integer, ByVal bytes() As Byte) As Boolean
        Try
            If Not IsNothing(ECO_Connect(Index)) Then
                Dim stream As NetworkStream = ECO_Connect(Index).GetStream
                stream.Write(bytes, 0, bytes.Length)
                Return True
            End If
            Return False
        Catch ex As Exception
            RaiseEvent OnError(Index, ex, Nothing, Nothing)
            Return False
        End Try
    End Function

#End Region

    Private Function StringConvertToBoolean(ByVal [String] As String, Optional ByVal [Delimiter] As String = ",") As Boolean()
        Dim Temp() As String = Split([String], Delimiter)
        Dim Temp1(UBound(Temp)) As Boolean
        For a = 0 To UBound(Temp)
            Temp1(a) = Temp(a)
        Next
        Return Temp1
    End Function

    Public Sub DoWork() Implements ITCPService.DoWork
    End Sub
End Class
