import { createBrowserRouter } from "react-router-dom";
import Cafes from "./pages/Cafes";
import Employees from "./pages/Employees";
import { CAFES_PATH, EMPLOYEES_PATH, ROOT_PATH } from "./shared/links";
import Root from "./pages/Root";

const router = createBrowserRouter([
  {
    path: ROOT_PATH,
    Component: Root,
    children: [
      {
        path: CAFES_PATH,
        Component: Cafes,
      },
      {
        path: EMPLOYEES_PATH,
        Component: Employees,
      }
    ],
  }
]);

export default router;