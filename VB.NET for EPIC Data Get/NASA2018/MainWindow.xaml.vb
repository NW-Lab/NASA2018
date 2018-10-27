Imports Newtonsoft.Json
Imports System.Net
Class MainWindow

    Dim ApiKey As String

    Dim url As String = "https://api.nasa.gov/EPIC/api/enhanced"

    Public Class EpicData
        Public Property identifier As String
        Public Property image As String
        Public Property [date] As DateTime
        Public Property Caption As String
        Public Property version As String
    End Class

    Private Sub Button1_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Dim date1 As Date = datepicker1.SelectedDate
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls12
        Dim http_req2 As Net.HttpWebRequest = Net.HttpWebRequest.Create(url & "/date/" & date1.Year & "-" & Right("00" & date1.Month, 2) & "-" & Right("00" & date1.Day, 2) & "?api_key=" & ApiKey)
        'Dim http_req2 As Net.HttpWebRequest = Net.HttpWebRequest.Create(url & "/date/2018-10-01?api_key=" & ApiKey)
        Dim http_res2 As Net.HttpWebResponse = http_req2.GetResponse
        Dim st As System.IO.Stream = http_res2.GetResponseStream()
        Dim sr As New System.IO.StreamReader(st, System.Text.Encoding.UTF8)
        Dim htmlSource As String = sr.ReadToEnd()
        sr.Close()
        st.Close()
        Dim epicdatas = JsonConvert.DeserializeObject(Of List(Of EpicData))(htmlSource)
        Dim epicarray As EpicData() = epicdatas.ToArray

        Console.WriteLine(url & "/date/" & date1.ToString("yyyy-MM-dd").Trim & "?api_key=" & ApiKey)
        Console.WriteLine(htmlSource)
        http_res2.Close()
        Console.WriteLine("n=" & epicarray.Length)

        If epicarray.Length = 0 Then
            MsgBox("No Data & " & date1.ToShortDateString)
            Return
        End If
        Console.WriteLine("url=" & epicarray(0).date)
        Dim ff As New System.Windows.Forms.FolderBrowserDialog
        ff.SelectedPath = My.Computer.FileSystem.SpecialDirectories.Desktop
        If ff.ShowDialog = Forms.DialogResult.OK Then
            Dim ffName As String = ff.SelectedPath
            For i = 0 To epicarray.Length - 1
                My.Computer.Network.DownloadFile("https://epic.gsfc.nasa.gov/archive/enhanced/" & epicarray(i).date.Year & "/" & Right("00" & epicarray(i).date.Month, 2) & "/" & Right("00" & epicarray(i).date.Day, 2) & "/png/" & epicarray(i).image & ".png", ffName & "/image" & Right("0" & i, 2) & ".png")
            Next
            MsgBox("File Download Finish")
        Else
            MsgBox("Cancel!")
        End If


        'http_req = Nothing

        ''  Dim http_req2 As Net.HttpWebRequest = Net.HttpWebRequest.Create(deserialized.url)
        ''  Dim http_req2 As Net.HttpWebRequest = Net.HttpWebRequest.Create("https://s.yimg.jp/images/top/sp2/cmn/logo-170307.png")
        ''  http_req2.Timeout = 10000
        ''  Dim http_res2 As Net.HttpWebResponse = http_req.GetResponse

        'Dim gr As BitmapImage = New BitmapImage(New Uri(deserialized.url), New Net.Cache.RequestCachePolicy(Net.Cache.RequestCacheLevel.CacheIfAvailable))

        '' Dim gr As New BitmapImage

        ' gr.StreamSource = http_res2.GetResponseStream

        'Dim gr As BitmapImage = New BitmapImage()
        ''Using web As New System.Net.Http.HttpClient()

        ''    Dim bytes As Byte() = Await web.GetByteArrayAsync(New Uri(deserialized.url))
        ''    Using stream As IO.MemoryStream = New IO.MemoryStream(bytes)

        ''        gr.BeginInit()
        ''        gr.StreamSource = stream
        ''        gr.CacheOption = BitmapCacheOption.OnLoad
        ''        gr.EndInit()
        ''        gr.Freeze()
        ''    End Using
        ''End Using
        'gr.BeginInit()
        'gr.UriSource = New Uri("https://apod.nasa.gov/apod/image/1810/NGC6543-BYU-L1024.jpg")
        'gr.EndInit()
        ''    'Do While gr.IsDownloading
        ''    '    System.Threading.Thread.Sleep(1000)
        ''    '    Console.WriteLine("downloging=" & gr.IsDownloading)
        ''    'Loop

        'abc.Source = gr

    End Sub

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        ApiKey = My.Settings.API_KEY
        tbApiKey.Text = ApiKey
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls12
        Dim http_req As Net.HttpWebRequest = Net.HttpWebRequest.Create(url & "?api_key=" & ApiKey)

        Dim http_res As Net.HttpWebResponse = http_req.GetResponse
        Dim st As System.IO.Stream = http_res.GetResponseStream()
        Dim sr As New System.IO.StreamReader(st, System.Text.Encoding.UTF8)
        Dim htmlSource As String = sr.ReadToEnd()
        sr.Close()
        st.Close()
        Dim epicdatas = JsonConvert.DeserializeObject(Of List(Of EpicData))(htmlSource)
        Dim epicarray As EpicData() = epicdatas.ToArray
        Console.WriteLine(htmlSource)
        Console.WriteLine("n=" & epicarray.Length)
        Console.WriteLine("url=" & epicarray(0).date)
        http_res.Close()
        http_req = Nothing
        ApiKey = My.Settings.API_KEY
        text1.Text = "Data Available From 2015/6/17 To " & epicarray(0).date.ToShortDateString & "   "
    End Sub


    Private Sub tbApiKey_TextChanged(sender As Object, e As TextChangedEventArgs) Handles tbApiKey.TextChanged
        ApiKey = tbApiKey.Text
    End Sub

    Private Sub btApiKey_Click(sender As Object, e As RoutedEventArgs) Handles btApiKey.Click
        My.Settings.API_KEY = tbApiKey.Text
        My.Settings.Save
    End Sub
End Class
