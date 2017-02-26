Public Class Engine
    Private _dataSet As DataSet
    Sub New(dataSet As DataSet)
        _dataSet = dataSet
    End Sub

    Public Function GenerateDecisionTree(dataSet As DataSet) As Boolean
        Try
            ' Compute the set entropy
            Dim setEntropy = ComputeSetEntropy(dataSet)

            ' Compute information gain
            dataSet = ComputeInformationGain(dataSet, setEntropy)
            dim someVal = 10
        Catch ex As Exception
            Return False
        End Try

        Return True
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
                    if dataItem.AttributeValues(i) = currentAttributeValue
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
    
    Public Function ComputePartialEntropy(oracleCategoryDictionaryList As List(Of Dictionary(Of String, Integer))) As Double
        for each oracleCategoryDictionary in oracleCategoryDictionaryList

        Next
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
    Public AttributeValues As New List(Of String)
End Class