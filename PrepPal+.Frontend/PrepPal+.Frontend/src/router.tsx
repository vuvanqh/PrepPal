import { createBrowserRouter } from "react-router-dom";
import Login from "./pages/Login";
import IntroLayout from "./pages/IntroLayout";
import Register from "./pages/Register";
import MainPage from "./pages/MainPage";
import AppLayoutPage from "./pages/AppLayoutPage";
import ProtectedRoute from "./components/ProtectedRoute";
import MainPageContent from "./components/MainPageContent";

const router = createBrowserRouter([
    {
        path:"/",
        element: <AppLayoutPage/>,
        children:[
            {
                path: "/",
                element: <IntroLayout/>,
                children: [ 
                    {
                        path: "login",
                        element: <Login/>,
                        children: []
                    },
                    {
                        path: "register",
                        element: <Register/>
                    }
                ]
            },
            {
                path:"main",
                element: <ProtectedRoute><MainPage/></ProtectedRoute>,
                children: [
                    {
                        index: true,
                        element: <MainPageContent/>

                    }
                ]
            }
        ]
    }
])

export default router;