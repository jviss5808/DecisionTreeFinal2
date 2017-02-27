Public Class Engine
    Private _dataSet As DataSet
    Dim _decisionTree As New List(Of NodeWithVertices)
    Sub New(dataSet As DataSet)
        _dataSet = dataSet
    End Sub

    Public Function PerformID3(trainingExamples As List(Of TrainingItem), trainingAttribute As TrainingAttribute) As Boolean

        ' Are all examples of the same class?


    End Function

    Public Function HolyShitDidThisWork() As Boolean
        Dim probablyNot = False

        Dim someNode = _decisionTree.Count
        Return probablyNot
    End Function

    Public Function GenerateDecisionTree(dataSet As DataSet, verticeICameFrom As String, treeLevel As Integer) As Boolean
        Try
            ' Are all examples of the same class?
            Dim commonClassLabel = CheckIfExamplesAreOfSameClass(dataSet)
            If (commonClassLabel <> String.Empty) Then
                ' return a leaf with common class label
                Dim nodeWithVertices As New NodeWithVertices
                nodeWithVertices.ClassValue = commonClassLabel
                nodeWithVertices.IsLeaf = true
                nodeWithVertices.TreeLevel = treeLevel
                nodeWithVertices.AttributeName = verticeICameFrom
                _decisionTree.Add(nodeWithVertices)

                ' Are we out of attributes to test?
            ElseIf (dataSet.AttributeList.Count = 0) Then
                ' return a leaf with common class label
                commonClassLabel = GetMostCommonClassLabel(dataSet)
                Dim nodeWithVertices As New NodeWithVertices
                nodeWithVertices.AttributeName = verticeICameFrom
                nodeWithVertices.IsLeaf = true
                nodeWithVertices.TreeLevel = treeLevel
                nodeWithVertices.ClassValue = commonClassLabel
                _decisionTree.Add(nodeWithVertices)
            Else

                ' Compute the set entropy
                Dim setEntropy = ComputeSetEntropy(dataSet)

                ' Compute information gain
                dataSet = ComputeInformationGain(dataSet, setEntropy)

                ' Find the attribute with the highest information gain
                Dim highestInformationGain as Double = 0
                Dim highestInformationGainIndex = -1
                For i = 0 To dataSet.AttributeList.Count - 1
                    If dataSet.AttributeList(i).InformationGain > highestInformationGain Then
                        highestInformationGain = dataSet.AttributeList(i).InformationGain
                        highestInformationGainIndex = i
                    End If
                Next

                ' Start building the tree
                Dim highestInformationGainAttribute = dataSet.AttributeList(highestInformationGainIndex)
                Dim nodeWithVertices As New NodeWithVertices
                nodeWithVertices.AttributeName = highestInformationGainAttribute.AttributeName
                For Each attributeValue In highestInformationGainAttribute.AttributeValues
                    nodeWithVertices.Vertices.Add(attributeValue)
                Next
                nodeWithVertices.TreeLevel = treeLevel
                _decisionTree.Add(nodeWithVertices)

                Dim sortedExamples As New List(Of TrainingItem)
                ' Recurse!
                For Each vertice In nodeWithVertices.Vertices
                    sortedExamples = GetSortedExamples(nodeWithVertices.AttributeName, vertice, dataSet)

                    ' Create a new dataset item and call recurse
                    Dim newDataSet as New DataSet
                    newDataSet.OracleCategories = dataSet.OracleCategories
                    newDataSet.AttributeList = dataSet.AttributeList
                    newDataSet.TrainingData = sortedExamples
                    GenerateDecisionTree(newDataSet, vertice, treeLevel + 1)
                Next

            End If

        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function

    Public Function GetSortedExamples(attributeName As String, attributeValue As String, dataSet As DataSet) As List(Of TrainingItem)
        Dim trainingData as New List(Of TrainingItem)
        for each trainingItem in dataSet.TrainingData
            for each trainingAttribute in trainingItem.AttributeValues
                if trainingAttribute.Name = attributeName AndAlso trainingAttribute.Value = attributeValue
                    trainingData.Add(trainingItem)
                End If
            Next
        Next

        return trainingData
    End Function

    Public Function GetMostCommonClassLabel(dataSet As DataSet)
        Dim currentHighestFrequency As Integer = 0
        Dim mostCommonClassLabel As String = String.Empty
        ' Query the frequency of all oracle values (categories) in the training data returning the highest 
        For Each oracleVal In dataSet.OracleCategories
            Dim categoryCount = dataSet.TrainingData.Where(Function(x) x.OracleValue = oracleVal).Count
            If categoryCount > currentHighestFrequency Then
                currentHighestFrequency = categoryCount
                mostCommonClassLabel = oracleVal
            End If
        Next
        Return mostCommonClassLabel
    End Function

    Public Function CheckIfExamplesAreOfSameClass(dataSet As DataSet) As String
        Dim commonClass As String = String.Empty

        ' If all the training values are of the same class, return that class
        If (dataSet.TrainingData.All(Function(x) x.OracleValue = dataSet.TrainingData(0).OracleValue)) Then
            commonClass = dataSet.TrainingData(0).OracleValue
        End If

        Return commonClass
    End Function

    Public Function ComputeInformationGain(dataSet As DataSet, setEntropy As Double) As DataSet

        ' Compute the information gain for each attribute
        For i = 0 To dataSet.AttributeList.Count - 1
            ' We're now ittering through attribute names (i.e. Wind)
            Dim currentAttribute = dataSet.AttributeList(i)


            Dim oracleCategoryDictionaryList As New List(Of Dictionary(Of String, Integer))

            For j = 0 To currentAttribute.AttributeValues.Count - 1
                ' We're now itterating through specific attributes, like Wind_Weak
                Dim currentAttributeValue = currentAttribute.AttributeValues(j)
                Dim oracleCategoryDictionary As New Dictionary(Of String, Integer)

                ' Instantiate the dictionary
                For Each oracleCategory In dataSet.OracleCategories
                    oracleCategoryDictionary.Add(oracleCategory, 0)
                Next
                oracleCategoryDictionary.Add("TotalCounts", 0)

                ' How many times did this current attribute value occur with each oracle category?
                For Each dataItem In dataSet.TrainingData
                    Dim oracleValue = dataItem.OracleValue
                    If dataItem.AttributeValues(i).Value = currentAttributeValue Then
                        oracleCategoryDictionary(oracleValue) += 1
                        oracleCategoryDictionary("TotalCounts") += 1
                    End If
                Next

                oracleCategoryDictionaryList.Add(oracleCategoryDictionary)

            Next


            Dim entropyList As New List(Of Double)
            Dim subsetSizeList As New List(Of Integer)
            For Each oracleCategoryDictionary In oracleCategoryDictionaryList
                Dim entropySum As Double = 0
                For Each key In oracleCategoryDictionary.Keys
                    If Not key.Contains("TotalCounts") Then
                        Dim p_i As Double = oracleCategoryDictionary(key) / oracleCategoryDictionary("TotalCounts")
                        Dim partialEntropy As Double = 0
                        ' Math library does like 0*log(0) or 1*log(1), as long as p_i isn't 1 or 0, compute it. Else, use the 
                        ' assigned value of 0
                        If Not (p_i = 1 Or p_i = 0) Then
                            partialEntropy = -(p_i * (Math.Log(p_i) / Math.Log(2)))
                        End If

                        entropySum += partialEntropy
                    End If
                Next
                subsetSizeList.Add(oracleCategoryDictionary("TotalCounts"))
                entropyList.Add(entropySum)
            Next

            ' We're actually computing the information gain at this point
            Dim informationGain As Double = setEntropy
            For j = 0 To entropyList.Count - 1
                informationGain -= (subsetSizeList(j) / dataSet.TrainingData.Count) * entropyList(j)
            Next

            dataSet.AttributeList(i).InformationGain = informationGain
        Next

        Return dataSet
    End Function


    Public Function ComputeSetEntropy(dataSet As DataSet) As Double


        ' We're going to go through each data item, look at the oracle value (this is the dictionary key)
        ' and keep track of the count of each particular oracle value
        Dim oracleCategoryDictionary As New Dictionary(Of String, Integer)

        ' Instantiate the dictionary
        For Each oracleCategory In dataSet.OracleCategories
            oracleCategoryDictionary.Add(oracleCategory, 0)
        Next

        ' Rip through all the training data to get number of oracle hits
        For Each dataItem In dataSet.TrainingData
            If oracleCategoryDictionary.ContainsKey(dataItem.OracleValue) Then
                oracleCategoryDictionary(dataItem.OracleValue) += 1
            Else
                MessageBox.Show("Found an oracle value that wasn't listed in the header! Oracle Value: " + dataItem.OracleValue)
            End If
        Next


        Dim numTrainingItems = dataSet.TrainingData.Count

        ' Compute entropy
        Dim entropy As Double = 0
        For Each oracleValue In oracleCategoryDictionary
            Dim p_i As Double = oracleValue.Value / numTrainingItems
            entropy += -(p_i * (Math.Log(p_i) / Math.Log(2)))
        Next

        Return entropy
    End Function

End Class

Public Class DataSet
    Public OracleCategories As New List(Of String)
    Public AttributeList As New List(Of TrainingAttribute)
    Public TrainingData As New List(Of TrainingItem)

End Class

Public Class TrainingAttribute
    Public AttributeName As String = String.Empty
    Public AttributeValues As New List(Of String)
    Public InformationGain As Double = 0
End Class

Public Class TrainingItem
    Public OracleValue As String = String.Empty
    Public AttributeValues As New List(Of AttributeNameValuePair)
End Class

Public Class AttributeNameValuePair
    Public Name As String = ""
    Public Value As String = ""

    Sub New(name As String, value As String)
        Me.Name = name
        Me.Value = value
    End Sub
End Class