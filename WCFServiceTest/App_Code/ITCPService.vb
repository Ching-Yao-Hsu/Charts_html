Imports System.ServiceModel
Imports TCPServer

' 注意: 若變更此處的類別名稱 "ITCPService"，也必須更新 Web.config 中 "ITCPService" 的參考。
<ServiceContract()> _
Public Interface ITCPService

    <OperationContract()> _
    Sub DoWork()

End Interface
