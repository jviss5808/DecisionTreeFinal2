Public Class Engine
    Private _dataSet As DataSet
    Dim _decisionTree As New List(Of NodeWithVertices)
    Sub New(dataSet As DataSet)
        _dataSet = dataSet
    End Sub

    Public Function PerformID3(trainingExamples As List(Of TrainingItem), trainingAttribute As TrainingAttribute) As Boolean

        ' Are all examples of the same class?


    End Function

    Public Function GenerateDecisionTree(dataSet As DataSet) As Boolean
        Try
            ' Are all examples of the same class?
            Dim commonClass = CheckIfExamplesAreOfSameClass(dataSet)
            if (commonClass <> string.Empty)
                ' return a leaf with common class label
            End If

            ' Are we out of attributes to test?
            if (dataSet.AttributeList.Count = 0)
                commonClass = GetMostCommonClass(dataSet)
                ' return a leaf with common class label
            End If

            ' Compute the set entropy
            Dim setEntropy = ComputeSetEntropy(dataSet)

            ' Compute information gain
            dataSet = ComputeInformationGain(dataSet, setEntropy)

            ' Find the attribute with the highest information gain
            Dim highestInformationGain = 0
            Dim highestInformationGainIndex = - 1
            for i = 0 to dataSet.AttributeList.Count - 1
                if dataSet.AttributeList(i).InformationGain > highestInformationGain
                    highestInformationGain = dataSet.AttributeList(i).InformationGain
                    highestInformationGainIndex = i
                End If
            Next

            ' Start building the tree
            Dim highestInformationGainAttribute = dataSet.AttributeList(highestInformationGainIndex)
            Dim nodeWithVertices as New NodeWithVertices
            nodeWithVertices.AttributeName = highestInformationGainAttribute.AttributeName
            for each attributeValue in highestInformationGainAttribute.AttributeValues
                nodeWithVertices.Vertices.Add(attributeValue)
            Next
            nodeWithVertices.TreeLevel = 1
            _decisionTree.Add(nodeWithVertices)

            Dim sortedExamples as New List(Of TrainingItem)
            for each vertice in nodeWithVertices.Vertices
                sortedExamples = GetSortedExamples(nodeWithVertices.AttributeName, vertice, dataSet)
                dim someNewVal = 11
            Next


            dim someVal = 10
        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function

    Public Function GetMostCommonClass(dataSet As DataSet)
        Dim currentHighestFrequency As Integer = 0
        Dim mostCommonClass as String = String.Empty
        ' Query the frequency of all oracle values (categories) in the training data returning the highest 
        for each oracleVal in dataSet.OracleCategories
            Dim categoryCount = dataSet.TrainingData.Where(Function(x) x.OracleValue = oracleVal).Count
            If categoryCount > currentHighestFrequency
                currentHighestFrequency = categoryCount
                mostCommonClass = oracleVal
            End If
        Next
        return mostCommonClass
    End Function

    Public Function CheckIfExamplesAreOfSameClass(dataSet As DataSet) As String
        Dim commonClass As String = string.Empty

        ' If all the training values are of the same class, return that class
        if (dataSet.TrainingData.All(Function(x) x.OracleValue = dataSet.TrainingData(0).OracleValue))
            commonClass = dataSet.TrainingData(0).OracleValue
        End If

        return commonClass
    End Function


    Public Function GetSortedExamples(attributeName As String, attributeValue As String, dataSet as DataSet) As List(Of TrainingItem)
        'for each trainingItem
    End Function

    Public Function ComputeInformationGain(dataSet As DataSet, setEntropy As Double) As DataSet

        ' Compute the information gain for each attribute
        for i = 0 To dataSet.AttributeList.Count - 1
            ' We're now ittering through attribute names (i.e. Wind)
            Dim currentAttribute = dataSet.AttributeList(i)


            Dim oracleCategoryDictionaryList as New List(Of Dictionary(Of String, Integer))

            for j = 0 to currentAttribute.AttributeValues.Count - 1
                ' We're now itterating through specific attributes, like Wind_Weak
                Dim currentAttributeValue = currentAttribute.AttributeValues(j)
                Dim oracleCategoryDictionary as New Dictionary(Of String, integer)
            
                ' Instantiate the dictionary
                For Each oracleCategory In dataSet.OracleCategories
                    oracleCategoryDictionary.Add(oracleCategory, 0)
                Next
                oracleCategoryDictionary.Add("TotalCounts", 0)

                ' How many times did this current attribute value occur with each oracle category?
                for each dataItem in dataSet.TrainingData
                    Dim oracleValue = dataItem.OracleValue
                    if dataItem.AttributeValues(i).Value = currentAttributeValue
                        oracleCategoryDictionary(oracleValue) += 1
                        oracleCategoryDictionary("TotalCounts") += 1
                    End If
                Next

                oracleCategoryDictionaryList.Add(oracleCategoryDictionary)

            Next

            
            Dim entropyList as New List(Of double)
            Dim subsetSizeList As New List(Of Integer)
            for each oracleCategoryDictionary In oracleCategoryDictionaryList
                Dim entropySum As Double = 0
                for each key in oracleCategoryDictionary.Keys
                    if not key.Contains("TotalCounts") Then
                        Dim p_i as Double = oracleCategoryDictionary(key)/oracleCategoryDictionary("TotalCounts")
                        Dim partialEntropy as Double = 0
                        ' Math library does like 0*log(0) or 1*log(1), as long as p_i isn't 1 or 0, compute it. Else, use the 
                        ' assigned value of 0
                        if Not (p_i = 1 or p_i = 0) 
                            partialEntropy = -(p_i * (Math.Log(p_i) / Math.Log(2)))
                        End If
                        
                        entropySum += partialEntropy
                    End If
                Next
                subsetSizeList.Add(oracleCategoryDictionary("TotalCounts"))
                entropyList.Add(entropySum)
            Next

            ' We're actually computing the information gain at this point
            dim informationGain As Double = setEntropy
            for j = 0 to entropyList.Count - 1
                informationGain -= (subsetSizeList(j)/dataSet.TrainingData.Count) * entropyList(j)
            Next

            dataSet.AttributeList(i).InformationGain = informationGain
        Next

        return dataSet
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

Public Class AttributeValueDictionary
    Public attributeValueDictionary As New Dictionary(Of String, Integer)
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

    Sub New (name as String, value As String)
        Me.Name = name
        Me.Value = value
    End Sub
End Class