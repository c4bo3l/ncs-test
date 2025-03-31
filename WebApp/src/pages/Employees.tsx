import { useQuery } from "@tanstack/react-query";
import { useState } from "react";
import useEmployeeApi from "../api/useEmployeeApi";
import { Button, Grid, IconButton, Stack } from "@mui/material";
import {
  DataGrid,
  GridColDef,
  GridFooter,
  GridFooterContainer,
} from "@mui/x-data-grid";
import GetEmployeeDto from "../models/getEmployeeDto";
import AddIcon from "@mui/icons-material/Add";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import EmployeeEditor from "../components/EmployeeEditor";
import "./DatagridHelper.css";
import EmployeeDeleteConfirmation from "../components/EmployeeDeleteConfirmation";
import CafeSelector from "../components/CafeSelector";

const Employees = () => {
  const [selectedCafe, setSelectedCafe] = useState<string>();
  const { getEmployees } = useEmployeeApi();
  const [dialog, setDialogs] = useState<{ editor: boolean; delete: boolean }>({
    editor: false,
    delete: false,
  });
  const [selectedEmployee, setSelectedEmployee] = useState<GetEmployeeDto>();

  const { data, isLoading, refetch } = useQuery({
    queryKey: ["employees", selectedCafe],
    queryFn: async () => await getEmployees(selectedCafe),
  });

  const toggleDialog =
    (dialogKey: "editor" | "delete", employee?: GetEmployeeDto) => () => {
      setSelectedEmployee(employee);
      setDialogs((prev) => ({
        ...prev,
        [dialogKey]: !prev[dialogKey],
      }));
    };

  const columns: GridColDef<GetEmployeeDto>[] = [
    {
      field: "name",
      flex: 2,
      headerName: "Name",
    },
    {
      field: "gender",
      flex: 1,
      headerName: "Gender",
      headerAlign: "center",
      align: "center",
      valueFormatter: (_, row) => (row.gender === "F" ? "Female" : "Male"),
    },
    {
      field: "email",
      flex: 2,
      headerName: "Email",
    },
    {
      field: "phone",
      flex: 2,
      headerName: "Phone",
    },
    {
      field: "cafe",
      flex: 2,
      headerName: "Cafe",
    },
    {
      field: "days_worked",
      flex: 2,
      headerName: "Days Worked",
    },
    {
      field: "action",
      flex: 1,
      headerName: "Actions",
      headerAlign: "center",
      align: "center",
      cellClassName: "center-content",
      sortable: false,
      renderCell: (params) => (
        <Stack direction="row" spacing={1}>
          <IconButton
            size="medium"
            color="warning"
            onClick={toggleDialog("editor", params.row)}
          >
            <EditIcon fontSize="inherit" />
          </IconButton>
          <IconButton
            size="medium"
            color="error"
            onClick={toggleDialog("delete", params.row)}
          >
            <DeleteIcon fontSize="inherit" />
          </IconButton>
        </Stack>
      ),
    },
  ];

  const customFooter = () => (
    <GridFooterContainer
      sx={(theme) => ({
        padding: `0 ${theme.spacing(1)}`,
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center",
      })}
    >
      <Button
        variant="contained"
        color="primary"
        startIcon={<AddIcon />}
        onClick={toggleDialog("editor")}
      >
        Add Employee
      </Button>
      <GridFooter
        sx={{
          border: "none",
        }}
      />
    </GridFooterContainer>
  );

  return (
    <>
      {dialog.delete && selectedEmployee ? (
        <EmployeeDeleteConfirmation
          employee={selectedEmployee}
          onCancel={toggleDialog("delete")}
          onDeleted={refetch}
        />
      ) : null}
      {dialog.editor ? (
        <EmployeeEditor
          value={selectedEmployee && { ...selectedEmployee }}
          onCancel={toggleDialog("editor")}
          onSaved={() => {
            refetch();
          }}
        />
      ) : null}
      <Grid container spacing={2}>
        <Grid
          size={{
            xs: 12,
            sm: 12,
            md: 4,
          }}
          offset={{
            sm: 0,
            md: 8,
          }}
        >
          <CafeSelector onChanged={(val) => setSelectedCafe(val || undefined)} />
        </Grid>
        <Grid size={12}>
          <DataGrid
            columns={columns}
            rows={data || []}
            loading={isLoading}
            sx={{
              height: "calc(100vh - 200px)",
            }}
            slots={{
              footer: customFooter,
            }}
          />
        </Grid>
      </Grid>
    </>
  );
};

export default Employees;
