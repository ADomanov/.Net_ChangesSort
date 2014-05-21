Imports System.IO
Imports System.Security.Cryptography
Imports System.Text

Public Class frmMain

    Private Class clsItem
        Public Property Title As String
        Public Property Description As String
    End Class

    Private EventsOutput As String = ""

    Private Sub ButtonOpen_Click(sender As Object, e As EventArgs) Handles btnAction.Click
        If OpenFileDialogMain.ShowDialog() = Windows.Forms.DialogResult.OK Then
            ProcessFile(OpenFileDialogMain.FileName)
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If Not String.IsNullOrEmpty(EventsOutput) AndAlso SaveFileDialogMain.ShowDialog = Windows.Forms.DialogResult.OK Then
            My.Computer.FileSystem.WriteAllText(SaveFileDialogMain.FileName, EventsOutput, True)
            AddLog(String.Format("File '{0}' saved.", SaveFileDialogMain.FileName))
        End If
    End Sub

    Private Sub AddLog(sMsg As String)
        tbLogs.AppendText(sMsg + vbCrLf)
    End Sub

    Private Function ProcessLines(tLines() As String, ByRef EventsList As Dictionary(Of Integer, List(Of clsItem)), ByRef StartIdx As Integer, ByRef sClosesList As String) As Integer
        Dim tNum As Integer = -1
        While StartIdx < tLines.Count AndAlso tLines(StartIdx).Trim = ""
            StartIdx += 1
        End While
        If StartIdx < tLines.Count Then
            If tLines(StartIdx).ToLower.StartsWith("'d") Then
                Dim tEvent As New clsItem
                tEvent.Title = tLines(StartIdx)
                StartIdx += 1
                Dim sNum As String = ""
                Dim idx As Integer = 2
                While idx < tEvent.Title.Length AndAlso tEvent.Title(idx) >= "0" AndAlso tEvent.Title(idx) <= "9"
                    sNum += tEvent.Title(idx)
                    idx += 1
                End While
                While StartIdx < tLines.Count AndAlso tLines(StartIdx).Trim <> "" AndAlso Not tLines(StartIdx).ToLower.StartsWith("'d")
                    tEvent.Description += tLines(StartIdx).Trim + vbCrLf
                    StartIdx += 1
                End While
                If Integer.TryParse(sNum, tNum) Then
                    If Not EventsList.ContainsKey(tNum) Then EventsList(tNum) = New List(Of clsItem)
                    Dim isClone As Boolean = False
                    For j As Integer = 0 To EventsList(tNum).Count - 1
                        If EventsList(tNum)(j).Description = tEvent.Description Then isClone = True
                    Next
                    If Not isClone Then EventsList(tNum).Add(tEvent) Else sClosesList += CStr(IIf(sClosesList = "", "", ",")) + tNum.ToString
                End If
            End If
        End If
        Return tNum
    End Function

    Private Sub ProcessFile(SFName As String)
        AddLog(String.Format("Read file '{0}'", SFName))
        Dim sOrigFile As String = My.Computer.FileSystem.ReadAllText(SFName)
        If sOrigFile.Contains("*** History ***") Then
            Dim sLines() As String = sOrigFile.Replace(vbCr, "").Split(CChar(vbLf))
            AddLog(String.Format("Loaded {0} lines. Processing…", sOrigFile.Count))
            Dim start As Integer = 35
            While start < sLines.Count AndAlso start >= 10 AndAlso Not sLines(start).StartsWith("' ==================")
                start -= 1
            End While
            If start = 10 Then
                AddLog("Sorry, but we can't find start of changes logs")
            Else

                Dim EventsHeader As String = ""
                Dim EventsMaxNum As Integer = -1
                Dim EventsList As New Dictionary(Of Integer, List(Of clsItem))

                btnSave.Enabled = False
                EventsOutput = ""

                Dim PrevStart As Integer = 0
                For i As Integer = 0 To start
                    EventsHeader += sLines(i) + vbCrLf
                Next

                start += 1
                Dim sMissing As String = ""
                Dim sDuplicates As String = ""
                Dim sClones As String = ""

                While start < sLines.Count AndAlso PrevStart <> start
                    PrevStart = start
                    Dim Num As Integer = ProcessLines(sLines, EventsList, start, sClones)
                    If Num > 0 AndAlso Num > EventsMaxNum Then EventsMaxNum = Num
                End While

                If start < sLines.Count Then
                    AddLog(String.Format("Can't read properly. Finished at line {0}", start + 1))
                Else

                    EventsOutput = EventsHeader
                    For i As Integer = EventsMaxNum To 1 Step -1
                        If EventsList.ContainsKey(i) AndAlso EventsList(i) IsNot Nothing AndAlso EventsList(i).Count > 0 Then
                            If EventsList(i).Count > 1 Then sDuplicates += CStr(IIf(sDuplicates = "", "", ",")) + i.ToString
                            For Each tItem As clsItem In EventsList(i)
                                EventsOutput += String.Format("{0}{1}{0}{2}", vbCrLf, tItem.Title, tItem.Description)
                            Next
                        Else
                            sMissing += CStr(IIf(sMissing = "", "", ",")) + i.ToString
                        End If
                    Next
                    btnSave.Enabled = True

                End If
                AddLog(String.Format("Processed {0} elements.", EventsMaxNum))
                If sMissing <> "" Then AddLog(String.Format("Missing: {0}", sMissing))
                If sDuplicates <> "" Then AddLog(String.Format("Duplicates: {0}", sDuplicates))
                If sClones <> "" Then AddLog(String.Format("Removed clones for: {0}", sClones))

                If btnSave.Enabled Then
                    Dim md5Hash As MD5 = MD5.Create()
                    If GetMd5Hash(md5Hash, sOrigFile) = GetMd5Hash(md5Hash, EventsOutput) Then
                        AddLog("(!) Nothing changed. No need to save")
                        btnSave.Enabled = False
                    Else
                        AddLog("(!) You can save sorted changes.")
                        btnSave.Focus()
                    End If
                End If

            End If
            sLines = Nothing
        Else
            AddLog("Error: Wrong file. Please choose properly 'SharedAssemblyInfo.vb' file.")
        End If
        AddLog("")
    End Sub

    Shared Function GetMd5Hash(ByVal md5Hash As MD5, ByVal input As String) As String
        Dim data As Byte() = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input))
        Dim sBuilder As New StringBuilder()
        Dim i As Integer
        For i = 0 To data.Length - 1
            sBuilder.Append(data(i).ToString("x2"))
        Next i
        Return sBuilder.ToString()
    End Function

    Private Sub frmMain_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        If My.Application.CommandLineArgs.Count > 0 Then
            Dim sParam As String = My.Application.CommandLineArgs(0).Trim
            If Not sParam.Contains("\") Then sParam = My.Computer.FileSystem.CombinePath(Directory.GetCurrentDirectory, sParam)
            If My.Computer.FileSystem.FileExists(sParam) Then ProcessFile(sParam)
        End If
    End Sub

End Class
