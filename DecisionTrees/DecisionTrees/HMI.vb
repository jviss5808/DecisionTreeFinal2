Imports System.IO

Public Class HMI
    Public _engine As Engine
    Private Sub btnReadData_Click(sender As Object, e As EventArgs) Handles btnReadData.Click
        ReadData(".\Data\fishing.data")
        'ReadData(".\Data\car.data")
        'ReadData(".\Data\iris.data")
    End Sub

    Private Sub ReadData(dataPath As String)
        Dim dataLines as New List(Of String)

        Using sr As New StreamReader(dataPath)
            While Not sr.EndOfStream
                dataLines.Add(sr.ReadLine)
            End While
        End Using

        Dim dataSet as DataSet = FormatData(dataLines)

        _engine = New Engine(dataSet)
        Dim result = _engine.GenerateDecisionTree(dataSet)

    End Sub

    Private Function FormatData(dataLines As List(Of String)) As DataSet
        Dim dataSet as New DataSet

        ' Get oracle information
        Dim oracleCategories = dataLines(1).Split(",")
        for each oracleCategory in oracleCategories
            dataSet.OracleCategories.Add(oracleCategory)
        Next

        ' Get the attribute information
        Dim numAttributes = dataLines(2)
        Dim attributeStartIndex = 3 ' We always start reading the attributes in on the 3rd line

        ' This itterates from the start of the attribute information up to # of data lines
        for i = attributeStartIndex to attributeStartIndex + numAttributes - 1
            Dim dataAttributeStringList = dataLines(i).Split(",").ToList
            Dim trainingAttribute as New TrainingAttribute

            ' The 0th item is the attribute name
            trainingAttribute.AttributeName = dataAttributeStringList(0)

            ' Here we are adding the attribute values
            for j = 2 to dataAttributeStringList.Count - 1
                trainingAttribute.AttributeValues.Add(dataAttributeStringList(j))
            Next

            ' Add the training attribute to the list
            dataSet.AttributeList.Add(trainingAttribute)
        Next

        Dim indexToStartReadingData = numAttributes + 4

        ' Read in the actual data
        for i = indexToStartReadingData to dataLines.Count - 1
            Dim dataValues = dataLines(i).Split(",").ToList()
            Dim trainingItem as New TrainingItem

            ' The last value is the oracle value
            trainingItem.OracleValue = dataValues(dataValues.Count - 1)

            ' Collect all the attribute values for this training item
            for j = 0 to dataValues.Count - 2
                trainingItem.AttributeValues.Add(New AttributeNameValuePair(dataSet.AttributeList(j).AttributeName, dataValues(j)))
            Next

            ' Add the training item to the training data
            dataSet.TrainingData.Add(trainingItem)
        Next

        Return dataSet
    End Function

End Class
