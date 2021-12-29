Imports System.Data
Imports System.Data.SqlClient

Public Class Functii
    Public Function introducere_linie_bar(ByVal id_masa As String, ByVal nr_nota As Integer,
                                          ByVal ospatar_id As Integer, ByVal id_divizie As Integer,
                                          ByVal nume_device As String, ByVal connStr As String) As Boolean
        Dim conn As New SqlConnection
        conn.ConnectionString = connStr

        Dim cmd As SqlCommand = New SqlCommand(
                "INSERT INTO Comanda_Bar( " &
                        "nr_bon,id_ospatar,id_masa,id_divizie," &
                        "discount,moment,flag,err_ecr,log_comp) " &
                " VALUES(" & nr_nota & "," & ospatar_id & "," & id_masa & "," & id_divizie &
                        ",0,getdate(),1,0,'" & nume_device & "')", conn)
        Try
            conn.Open()
            cmd.ExecuteNonQuery()
            conn.Close()
        Catch ex As SqlException
            Return False
        End Try

        Return True
    End Function

    'Sub trimite_la_imprimanta(ByVal connStr As String, ByVal id_divizie As Integer)
    '    Dim codgest As DataTable = New DataTable ' = dtcomanda.DefaultView.ToTable("cod", True, "codg")
    '    Dim ipimprimantaN As String
    '    Dim ipimprimantaC As String
    '    Dim ipimprimanta As String

    '    'citeste imprimanta pentru tiparire comanda daca se listeaza precomanda (imprimanta pentru nota de plata)
    '    Dim conn As New SqlConnection
    '    conn.ConnectionString = connStr
    '    Dim cmd0 As New SqlCommand("select path_tiparire from divizie where id_divizie=" & id_divizie & "", conn)
    '    Try
    '        conn.Open()
    '        ipimprimantaN = cmd0.ExecuteScalar
    '        conn.Close()

    '        If ipimprimantaN = "" Then
    '            Exit Sub
    '        End If
    '    Catch ex As Exception
    '        ipimprimantaN = ""
    '    End Try

    '    For Each row As Data.DataRow In codgest.Rows

    '        'citeste pentru fiecare gestiune daca are specificata imprimanta pentru tiparire comanda
    '        Dim cmd As New SqlCommand("select path_tiparire from divizie_gestiune where id_divizie=" & id_divizie & " and id_gestiune=" & row(0) & "", conn)
    '        Try
    '            ipimprimantaC = cmd.ExecuteScalar

    '            If ipimprimantaC = "" Then
    '                Exit Sub
    '            End If
    '        Catch ex As Exception
    '            ipimprimantaC = ""
    '        End Try

    '        'incarca pe rand comanda pentru fiecare gestiune
    '        incarca_transa(row(0))

    '        ' creare si tiparire PRE/CDA
    '        ipimprimanta = ipimprimantaC
    '        If dtimprim.Rows.Count > 0 Then
    '            If list_precomanda = 1 Then 'listare precomanda
    '                creaza_bon("PRE", row(0))
    '                If ipimprimantaC = "" Then ipimprimanta = "" Else ipimprimanta = ipimprimantaN
    '                creaza_bon("CDA", row(0))
    '            Else
    '                creaza_bon("CDA", row(0))
    '            End If
    '        End If

    '    Next

    'End Sub

    Private Sub incarca_transa(ByVal codg As Integer, ByVal ospatar_id As Integer,
                               ByVal connStr As String, ByVal masa_id As Integer,
                               Optional ByVal idbon As Long = 0, Optional ByVal idtransa As Long = 0)
        Dim id_bon As Long
        Dim id_transa As Long
        Dim conn As New SqlConnection
        conn.ConnectionString = connStr

        'idbon poate fi trimis la anulare comanda unde nu e neaparat ultima id_transa
        If idtransa = 0 Then
            Dim cmd1 As New SqlCommand("select max(id_transa) from marfat where id_ospatar=" & ospatar_id & " and id_masa=" & masa_id & " and codgestiune=" & codg & "", conn)
            Try
                conn.Open()
                id_transa = cmd1.ExecuteScalar
                conn.Close()
                cmd1.Dispose()
            Catch ex As Exception

                id_transa = 0
            End Try
        Else
            id_transa = idtransa
        End If

        If idbon = 0 Then
            Dim cmd As New SqlCommand("select max(id_bon) from comanda_bar where id_ospatar=" & ospatar_id & " and id_masa=" & masa_id & " ", conn)
            Try
                conn.Open()
                id_bon = cmd.ExecuteScalar
                conn.Close()
                cmd.Dispose()
            Catch ex As Exception
            End Try
        Else
            id_bon = idbon
        End If

        Dim cmd2 As New SqlCommand()
        cmd2.Connection = conn
        cmd2.CommandType = Data.CommandType.StoredProcedure
        cmd2.CommandText = "R_CDA_TXT"
        With cmd2.Parameters.AddWithValue("@idbon", id_bon)
            .Direction = Data.ParameterDirection.Input
            .DbType = Data.DbType.Int32
        End With
        With cmd2.Parameters.AddWithValue("@idtransa", id_transa)
            .Direction = Data.ParameterDirection.Input
            .DbType = Data.DbType.Int32
        End With
        With cmd2.Parameters.AddWithValue("@sectie", codg)
            .Direction = Data.ParameterDirection.Input
            .DbType = Data.DbType.Int32
        End With

        Dim dtimprim = New Data.DataTable("impr")
        Dim adap As New SqlDataAdapter(cmd2)
        conn.Open()
        adap.Fill(dtimprim)
        conn.Close()
        adap.Dispose()
    End Sub

    'Public Function scrie_linii_marfat() As Boolean
    '    Dim cmd As Data.SqlClient.SqlCommand
    '    Dim i As Integer = 0
    '    Dim n As Integer = -1

    '    SW_verificare_stoc = 0

    '    cmd = New Data.SqlClient.SqlCommand()
    '    If mod_de_lucru = 0 Then
    '        cmd.CommandText = "select max(id_bon) from comanda_bar where id_ospatar=" & ospatar_id & " and id_divizie=" & id_divizie & ""
    '        cmd.Connection = conn
    '        Try
    '            n = cmd.ExecuteScalar
    '        Catch ex As Exception
    '            n = 0
    '            'MsgBox("Nu exista conexiune la baza de date", MsgBoxStyle.Critical)
    '            MsgBox("Exista comanda deschisa pe un alt casier", MsgBoxStyle.Critical)
    '            Exit Function
    '        End Try
    '    Else
    '        cmd.CommandText = "select max(id_bon) from comanda_bar where id_ospatar=" & ospatar_id & " and id_masa=" & masa_id & " and id_divizie=" & id_divizie & ""
    '        cmd.Connection = conn

    '        Try
    '            n = cmd.ExecuteScalar
    '        Catch ex As Exception
    '            n = 0
    '            MsgBox("Nu exista conexiune la baza de date", MsgBoxStyle.Critical)
    '            Exit Function
    '        End Try
    '    End If
    '    id_bon = n
    '    Dim dtgest As New Data.DataTable("gest")
    '    dtgest = dtcomanda.DefaultView.ToTable("gest", True, "codg")
    '    For Each rowgest As Data.DataRow In dtgest.Rows
    '        Dim prima_linie As Integer = 0
    '        Dim l As Integer = 0
    '        For Each row As Data.DataRow In dtcomanda.Rows
    '            If row("codg").Equals(rowgest("codg")) Then
    '                If l = 0 Then
    '                    prima_linie = 1
    '                    l = 1
    '                Else
    '                    prima_linie = 0
    '                End If
    '                'Dim row2 As Data.DataRow() = marfa_v_MP.Select("id= " & row("secventa") & " and codgest=" & row("codg") & "")
    '                Dim row2 As Data.DataRow() = marfa_v_MP.Select("PR=" & row("PR") & " AND id= " & row("secventa") & " and codgest=" & row("codg") & "")

    '                If row2(0).Item("um").ToString() = "LEI" Then
    '                    row2(0).Item("p_v") = row("pret")
    '                End If

    '                cmd = New Data.SqlClient.SqlCommand()
    '                Try
    '                    cmd.Connection = conn
    '                    cmd.CommandText = "EXEC marfaT_app "
    '                    cmd.CommandText = cmd.CommandText & " " & id_bon & ", "
    '                    'cmd.CommandText = cmd.CommandText & " " & nr_bon & ", "
    '                    cmd.CommandText = cmd.CommandText & " " & Me.NrNota.Text & ", "
    '                    cmd.CommandText = cmd.CommandText & " " & ospatar_id & ", "
    '                    cmd.CommandText = cmd.CommandText & " " & masa_id & ", "
    '                    cmd.CommandText = cmd.CommandText & " " & id_divizie & ", "
    '                    cmd.CommandText = cmd.CommandText & " '" & row2(0).Item("pr") & "+" & row2(0).Item("id") & "', "
    '                    cmd.CommandText = cmd.CommandText & " '" & row2(0).Item("denumire") & "', "
    '                    cmd.CommandText = cmd.CommandText & " " & row("cantitate").ToString.Replace(",", ".") & ", "
    '                    cmd.CommandText = cmd.CommandText & " " & row("codg") & " , "
    '                    cmd.CommandText = cmd.CommandText & " " & row("secventa") & " , "
    '                    cmd.CommandText = cmd.CommandText & " '" & row2(0).Item("codbare") & "', "
    '                    cmd.CommandText = cmd.CommandText & " '" & row2(0).Item("um") & "'" & " ,"
    '                    cmd.CommandText = cmd.CommandText & " '" & row2(0).Item("tva") & "'" & " , "
    '                    cmd.CommandText = cmd.CommandText & "  " & row2(0).Item("p_v").ToString.Replace(",", ".") & " , "
    '                    cmd.CommandText = cmd.CommandText & " " & row2(0).Item("p_v_ft").ToString.Replace(",", ".") & " , "
    '                    cmd.CommandText = cmd.CommandText & " " & row2(0).Item("p_s").ToString.Replace(",", ".") & " , "
    '                    cmd.CommandText = cmd.CommandText & " " & (row2(0).Item("p_v") * row("cantitate")).ToString.Replace(",", ".") & " , "
    '                    cmd.CommandText = cmd.CommandText & " " & row2(0).Item("pr") & " , "
    '                    cmd.CommandText = cmd.CommandText & " '" & row("specificatie") & "'  , " 'specificatie
    '                    cmd.CommandText = cmd.CommandText & " " & IIf(row2(0).Item("Disc") Is DBNull.Value, 0, row2(0).Item("Disc").ToString.Replace(",", ".")) & ","
    '                    cmd.CommandText = cmd.CommandText & " 0 , " 'marfa
    '                    cmd.CommandText = cmd.CommandText & " 0 , " 'stare
    '                    cmd.CommandText = cmd.CommandText & " 0 , " 'provenienta pocket
    '                    cmd.CommandText = cmd.CommandText & " " & prima_linie & ","
    '                    cmd.CommandText = cmd.CommandText & " " & Integer.Parse(row("loc")) & ","
    '                    cmd.CommandText = cmd.CommandText & "'" & row2(0).Item("p") & "', "
    '                    cmd.CommandText = cmd.CommandText & " " & row2(0).Item("pret_promo").ToString.Replace(",", ".") & ""
    '                    cmd.ExecuteNonQuery()
    '                    cmd.Dispose()
    '                Catch ex As Exception
    '                    MsgBox("Salvarea produsului nu s-a putut efectua")
    '                End Try
    '            End If
    '        Next

    '        If avertizare_stoc = 1 Then
    '            Dim idcom As Integer = -1
    '            Dim cmdv As New SqlClient.SqlCommand("select max(id_comanda) from marfat where id_divizie=" & id_divizie & " and id_masa=" & masa_id & " and codgestiune=" & rowgest("codg") & " and [id bon]=" & id_bon & "", conn)
    '            Try
    '                idcom = cmdv.ExecuteScalar
    '            Catch ex As Exception
    '                MsgBox("Eroare de verificare stoc.Id comanda nu exista", MsgBoxStyle.Exclamation)
    '                Return False
    '                Exit Function
    '            End Try
    '            If verifica_stoc(Integer.Parse(id_bon), idcom) = False Then SW_verificare_stoc = 1
    '        End If

    '    Next

    '    If SW_verificare_stoc = 1 Then
    '        Return False
    '        Exit Function
    '    End If

    '    trimite_la_imprimanta()
    '    dtcomanda.Clear()
    '    Return True

    'End Function
End Class
