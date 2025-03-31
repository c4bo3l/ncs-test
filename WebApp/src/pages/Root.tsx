import { Box, CssBaseline } from "@mui/material";
import NavBar from "../components/NavBar";
import { Navigate, Outlet, useLocation } from "react-router-dom";
import { CAFES_PATH, ROOT_PATH } from "../shared/links";

const Root = () => {
  const location = useLocation();
  
  if (location.pathname === ROOT_PATH) {
    return <Navigate to={CAFES_PATH} />;
  }
  
  return (
    <Box sx={{ display: "flex" }}>
        <CssBaseline />
        <NavBar />
        <Box
          sx={(theme) => ({
            p: theme.spacing(2),
            marginTop: theme.spacing(6),
            height: `calc(100vh - ${theme.spacing(12)})`,
            overflow: "auto",
            width: "100%",
          })}
        >
          <Outlet />
        </Box>
      </Box>
  );
};

export default Root;
