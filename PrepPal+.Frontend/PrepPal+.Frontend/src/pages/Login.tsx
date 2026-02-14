import Input from "../components/UI/Input"
import Button from "../components/UI/Button"
import Modal from "../components/Modal";
import { useNavigate, useLocation } from "react-router-dom";

export default function Login(){
    const navigate = useNavigate();
    const location = useLocation();
    const isLoginOpen = location.pathname == "/login";
    return (
    <Modal open={isLoginOpen} onClose={()=>navigate("/")}>
        <form className="login-form bg-stone-100 border-stone-950">
            <Input label="Email" id="email"/>
            <Input label="Password" id="password"/>
            <p className="form-actions">
                <Button text="Cancel" className="px-2 text-stone-500" type="button" onClick={()=>navigate("/")}/>
                <Button text="Log In" className="login-form-button bg-stone-950 w-15 text-stone-300 mr-0.5 rounded-md"/>
            </p>
        </form>
    </Modal>)
}