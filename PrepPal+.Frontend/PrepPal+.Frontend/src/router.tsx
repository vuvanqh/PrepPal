import { createBrowserRouter } from "react-router-dom";
import Login from "./pages/Login";
import IntroLayout from "./pages/IntroLayout";
import Register from "./pages/Register";

const router = createBrowserRouter([
    {
        path:"/",
        element: <IntroLayout/>,
        children:[
            {
                path: "login",
                element: <Login/>
            },
            {
                path: "register",
                element: <Register/>
            }
        ]
    }
])

export default router;