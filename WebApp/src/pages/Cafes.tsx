import { useQuery } from "@tanstack/react-query";
import { useState } from "react";
import useCafeApi from "../api/useCafeApi";
import { Button, Grid, IconButton, Stack, TextField } from "@mui/material";
import {
  DataGrid,
  GridColDef,
  GridFooter,
  GridFooterContainer,
} from "@mui/x-data-grid";
import GetCafeDto from "../models/getCafeDto";
import AddIcon from "@mui/icons-material/Add";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import CafeEditor from "../components/CafeEditor";
import "./DatagridHelper.css";
import CafeDeleteConfirmation from "../components/CafeDeleteConfirmation";

const Cafes = () => {
  const [cafeLocation, setCafeLocation] = useState<string>();
  const { getCafes } = useCafeApi();
  const [dialog, setDialogs] = useState<{ editor: boolean; delete: boolean }>({
    editor: false,
    delete: false,
  });
  const [selectedCafe, setSelectedCafe] = useState<GetCafeDto>();

  const { data, isLoading, refetch } = useQuery({
    queryKey: ["cafes", cafeLocation],
    queryFn: () => getCafes(cafeLocation),
  });

  const toggleDialog =
    (dialogKey: "editor" | "delete", cafe?: GetCafeDto) => () => {
      setSelectedCafe(cafe);
      setDialogs((prev) => ({
        ...prev,
        [dialogKey]: !prev[dialogKey],
      }));
    };

  const columns: GridColDef<GetCafeDto>[] = [
    {
      field: "logo",
      flex: 1,
      headerName: "Logo",
      headerAlign: "center",
      align: "center",
      cellClassName: "center-content",
      renderCell: (params) =>
        params.row.logo ? (
          <img src={params.row.logo} width={36} height={36} />
        ) : null,
    },
    {
      field: "name",
      flex: 2,
      headerName: "Name",
    },
    {
      field: "description",
      flex: 3,
      headerName: "Description",
    },
    {
      field: "location",
      flex: 2,
      headerName: "Location",
    },
    {
      field: "employees",
      flex: 1,
      headerName: "Employees",
      headerAlign: "left",
      align: "left",
      valueFormatter: (_, row) => row.employees.toLocaleString(),
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
        Add Cafe
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
      {dialog.delete && selectedCafe ? (
        <CafeDeleteConfirmation
          cafe={selectedCafe}
          onCancel={toggleDialog("delete")}
          onDeleted={refetch}
        />
      ) : null}
      {dialog.editor ? (
        <CafeEditor
          value={selectedCafe && { ...selectedCafe }}
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
          <TextField
            fullWidth
            label="Location"
            value={cafeLocation}
            onChange={(e) => setCafeLocation(e.currentTarget.value)}
          />
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

export default Cafes;
