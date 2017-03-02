<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HMI
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.btnReadData = New System.Windows.Forms.Button()
        Me.GraphViewer = New Microsoft.Glee.GraphViewerGdi.GViewer()
        Me.TableLayoutPanel1.SuspendLayout
        Me.SuspendLayout
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50!))
        Me.TableLayoutPanel1.Controls.Add(Me.btnReadData, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.GraphViewer, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 81.57895!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18.42105!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(572, 532)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'btnReadData
        '
        Me.btnReadData.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnReadData.Location = New System.Drawing.Point(3, 437)
        Me.btnReadData.Name = "btnReadData"
        Me.btnReadData.Size = New System.Drawing.Size(566, 92)
        Me.btnReadData.TabIndex = 0
        Me.btnReadData.Text = "Read Data"
        Me.btnReadData.UseVisualStyleBackColor = true
        '
        'GraphViewer
        '
        Me.GraphViewer.AsyncLayout = false
        Me.GraphViewer.AutoScroll = true
        Me.GraphViewer.BackwardEnabled = false
        Me.GraphViewer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GraphViewer.ForwardEnabled = false
        Me.GraphViewer.Graph = Nothing
        Me.GraphViewer.Location = New System.Drawing.Point(3, 3)
        Me.GraphViewer.MouseHitDistance = 0.05R
        Me.GraphViewer.Name = "GraphViewer"
        Me.GraphViewer.NavigationVisible = true
        Me.GraphViewer.PanButtonPressed = false
        Me.GraphViewer.SaveButtonVisible = true
        Me.GraphViewer.Size = New System.Drawing.Size(566, 428)
        Me.GraphViewer.TabIndex = 2
        Me.GraphViewer.ZoomF = 1R
        Me.GraphViewer.ZoomFraction = 0.5R
        Me.GraphViewer.ZoomWindowThreshold = 0.05R
        '
        'HMI
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(572, 532)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Name = "HMI"
        Me.Text = "DecisionTrees"
        Me.TableLayoutPanel1.ResumeLayout(false)
        Me.ResumeLayout(false)

End Sub

    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents btnReadData As Button
    Friend WithEvents GraphViewer As Microsoft.Glee.GraphViewerGdi.GViewer
End Class
