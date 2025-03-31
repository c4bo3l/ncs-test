import {
  createTheme,
  CssBaseline,
  StyledEngineProvider,
  ThemeProvider,
} from "@mui/material";
import { LocalizationProvider } from "@mui/x-date-pickers";
import { SnackbarProvider } from "notistack";
import { AdapterMoment } from "@mui/x-date-pickers/AdapterMoment";
import { RouterProvider } from "react-router-dom";
import router from "./router";
import "./App.css";
import { QueryClientProvider } from "@tanstack/react-query";
import queryClient from "./api/queryClient";

const theme = createTheme({
  palette: {
    mode: "light",
  },
});

const App = () => {
  return (
    <QueryClientProvider client={queryClient}>
      <StyledEngineProvider injectFirst>
        <SnackbarProvider>
          <ThemeProvider theme={theme}>
            <CssBaseline />
            <LocalizationProvider dateAdapter={AdapterMoment}>
              <RouterProvider router={router} />
            </LocalizationProvider>
          </ThemeProvider>
        </SnackbarProvider>
      </StyledEngineProvider>
    </QueryClientProvider>
  );
};

export default App;
