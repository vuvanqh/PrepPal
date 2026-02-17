import { createBrowserRouter } from "react-router-dom";
import Login from "./pages/Public/Login";
import IntroLayout from "./pages/Public/IntroLayout";
import Register from "./pages/Public/Register";
import MainPage from "./pages/Protected/MainPage";
import AppLayoutPage from "./pages/AppLayoutPage";
import ProtectedRoute from "./components/ProtectedRoute";
import MainPageContent from "./components/MainPageContent";
import SearchPage from "./pages/Protected/SearchPage";

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

                    },
                    {
                        path: "search/:recipeName",
                        element: <SearchPage/>
                    }
                ]
            }
        ]
    }
])

export default router;