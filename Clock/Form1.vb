Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.InteropServices

Public Class Form1
    Private check As Boolean = False
    Private Const WM_NCLBUTTONDOWN As Integer = &HA1
    Private Const HT_CAPTION As Integer = &H2

    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Private Shared Function SendMessage(hWnd As IntPtr,
    Msg As Integer, wParam As IntPtr, lParam As IntPtr) As IntPtr
    End Function
    <DllImportAttribute("user32.dll")>
    Private Shared Function ReleaseCapture() As Boolean
    End Function


    'Form1のMouseDownイベントハンドラ
    Private Sub Form1_MouseDown(sender As Object,
        e As System.Windows.Forms.MouseEventArgs) _
        Handles MyBase.MouseMove
        If e.Button = MouseButtons.Left Then
            'マウスのキャプチャを解除
            ReleaseCapture()
            'タイトルバーでマウスの左ボタンが押されたことにする
            SendMessage(Handle, WM_NCLBUTTONDOWN,
                    New IntPtr(HT_CAPTION), IntPtr.Zero)
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.DoubleBuffered = True

        Me.SetBounds(Me.Left, Me.Top, ofsX * 2, ofsY * 2,
                    BoundsSpecified.Size)

        ' フォームのクライアント領域のサイズを取得する
        Dim tSize As System.Drawing.Size = Me.ClientSize

        ' 取得したフォームのクライアント領域のサイズを表示する
        'MessageBox.Show(tSize.ToString())

        ' フォームのクライアント領域のサイズを設定する
        Me.ClientSize = New System.Drawing.Size(ofsX * 2, ofsY * 2)



        Dim path As New System.Drawing.Drawing2D.GraphicsPath()
        '丸を描く
        'タイトルバーの高さ
        Dim Ht = SystemInformation.CaptionHeight
        Dim Bd = SystemInformation.Border3DSize.Width + SystemInformation.BorderSize.Width
        '3D境界線のサイズ(ピクセル単位)
        Console.WriteLine("Border3DSize:{0}", SystemInformation.Border3DSize)

        Bd *= 3

        path.AddEllipse(New Rectangle(0 + Bd, 0 + Ht + Bd, ofsX * 2 - 3, ofsY * 2 - 3))
        '真ん中を丸くくりぬく
        'path.AddEllipse(New Rectangle(30, 30, 40, 40))
        Me.Region = New Region(path)


        Me.TopMost = False
        '
    End Sub

    '↓ダブルバッファー。タブコントロールが多すぎると不具合出るらしい。
    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            '拡張ウィンドウスタイルにWS_EX_COMPOSITEDを追加する
            cp.ExStyle = cp.ExStyle Or &H2000000
            Return cp
        End Get
    End Property

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Dim g As Graphics = Form1.CreateGraphics
        'g.DrawLine(Pens.Red, 0, 0, 100, 200)
    End Sub


    Const ofsX = 70
    Const ofsY = 70
    Private Sub Form1_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        'e.Graphics.DrawLine(New System.Drawing.Pen(System.Drawing.Color.Black),
        'New System.Drawing.PointF(0, 0), New System.Drawing.PointF(100, 100))

        Dim dn As DateTime = DateTime.Now

        Dim sc As Decimal = (dn.Second + dn.Millisecond / 1000) * 6
        If Timer1.Interval = 1000 Then
            sc = (dn.Second) * 6
        End If
        Dim hu As Decimal = dn.Minute * 6 + (dn.Second + dn.Millisecond / 1000) * 0.1
        Dim hr As Decimal = dn.ToString("%h") * 30 + dn.Minute * 0.5
        'Label1.Text = $"{mi \ 6}][{mi}"

        'e.Graphics.DrawString(dn.ToString("yyyy/MM/dd hh:mm:ss.fff tt"),
        '                      New Font("ＭＳ Ｐ明朝", 12, FontStyle.Italic),
        '                        Brushes.Red, New System.Drawing.Point(100, 200))

        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        e.Graphics.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality

        sc -= 90
        Dim x, y As Decimal
        x = (Math.Cos(Rad(sc)) + 1) * ofsX / 1.15
        y = (Math.Sin(Rad(sc)) + 1) * ofsY / 1.15
        e.Graphics.DrawLine(New System.Drawing.Pen(System.Drawing.Color.Black), New System.Drawing.PointF(ofsX, ofsY), New System.Drawing.PointF(ofsX - ofsX / 1.15 + x, ofsY - ofsY / 1.15 + y))

        sc -= 180
        x = (Math.Cos(Rad(sc)) + 1) * ofsX / 5
        y = (Math.Sin(Rad(sc)) + 1) * ofsY / 5
        'Debug.WriteLine($"x:{x},y:{y}")
        e.Graphics.DrawLine(New System.Drawing.Pen(System.Drawing.Color.Black), New System.Drawing.PointF(ofsX, ofsY), New System.Drawing.PointF(ofsX - ofsX / 5 + x, ofsY - ofsY / 5 + y))


        hu -= 90
        x = (Math.Cos(Rad(hu)) + 1) * ofsX / 1.25
        y = (Math.Sin(Rad(hu)) + 1) * ofsY / 1.25
        e.Graphics.DrawLine(New System.Drawing.Pen(System.Drawing.Color.Black, 2), New System.Drawing.PointF(ofsX, ofsY), New System.Drawing.PointF(ofsX - ofsX / 1.25 + x, ofsY - ofsY / 1.25 + y))

        hr -= 90
        x = (Math.Cos(Rad(hr)) + 1) * ofsX / 1.6
        y = (Math.Sin(Rad(hr)) + 1) * ofsY / 1.6
        e.Graphics.DrawLine(New System.Drawing.Pen(System.Drawing.Color.Black, 4), New System.Drawing.PointF(ofsX, ofsY), New System.Drawing.PointF(ofsX - ofsX / 1.6 + x, ofsY - ofsY / 1.6 + y))

        Dim sz As Single = 8
        e.Graphics.FillEllipse(Brushes.Gray, ofsX - sz / 2, ofsY - sz / 2, sz, sz)

        Label2.Text = x
        Label3.Text = y
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Me.Invalidate()
    End Sub

    Function Rad(x)
        Rad = x / 180 * Math.PI
    End Function

    Private Sub Form1_MouseClick(sender As Object, e As MouseEventArgs) Handles Me.MouseClick
        Select Case e.Button
            Case MouseButtons.Right
                Timer1.Interval = If(Timer1.Interval = 17, 1000, 17)
        End Select
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        'コントロールの外観を描画するBitmapの作成
        Dim bmp As New Bitmap(Me.Width, Me.Height)
        Dim bmp2 As New Bitmap(Me.Width, Me.Height)
        Dim g As Graphics = Graphics.FromImage(bmp2)

        '楕円の領域を追加する
        Dim gp As New System.Drawing.Drawing2D.GraphicsPath()
        Dim Ht = SystemInformation.CaptionHeight
        Dim Bd = SystemInformation.Border3DSize.Width + SystemInformation.BorderSize.Width
        '3D境界線のサイズ(ピクセル単位)

        Bd *= 3

        'gp.AddEllipse(New Rectangle(0 + Bd, 0 + Ht + Bd, ofsX * 2 - 3, ofsY * 2 - 3))
        gp.AddEllipse(g.VisibleClipBounds)
        'Regionを作成する
        Dim rgn As New Region(gp)
        'クリッピング領域を変更する
        g.Clip = rgn

        Me.DrawToBitmap(bmp, New Rectangle(0, 0, Me.Width, Me.Height))
        g.DrawImage(bmp, g.VisibleClipBounds)


        'キャプチャする
        'Me.DrawToBitmap(bmp, New Rectangle(0, 0, Me.Width, Me.Height))
        'ファイルに保存する
        'bmp2.Save("C:\temp\1.png")
        Dim img As Image = bmp2
        'img.Save("C:\temp\1.png")
        Using bmpp As New Bitmap(img)
            Using ico As Icon = Icon.FromHandle(bmpp.GetHicon())
                Me.Icon = ico
            End Using
        End Using


        '後始末
        bmp.Dispose()
        bmp2.Dispose()
    End Sub
End Class
