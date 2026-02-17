import Input from "../../components/UI/Input"
import Button from "../../components/UI/Button"
import Modal from "../../components/Modals/Modal";
import { useNavigate, useLocation } from "react-router-dom";
import {useMutation} from "@tanstack/react-query";
import useAuth from "../../customHooks/useAuth";
import { toastError, toastSuccess } from "../../toastConfig";
import { useActionState } from "react";
import ErrorContainer from "../../components/Errors/ErrorContainer";
import Error from "../../components/Errors/Error";

type loginFormState = {
  errors?: string[];
  email?: string;
};


export default function Login(){
    const navigate = useNavigate();
    const location = useLocation();
    const isLoginOpen = location.pathname == "/login";

    const {login} = useAuth();

    const {mutateAsync, isPending} = useMutation({
        mutationFn: login,
        onSuccess: ()=> {
            toastSuccess("Successful Login");
            navigate("/main");
        },
        onError: () =>{toastError("from login")}
    })

    async function submitAction(_: loginFormState, formData: FormData){
        const data = {
            "Email": formData.get("Email") as string,
            "Password": formData.get("Password") as string
        };

        try{
            const resp = await mutateAsync(data);
            console.log(resp);
            return {};
        }
        catch(err: any){
            toastError("Login Failed. Try Again :c"); 
            const raw = err.info?.errors;
            
            if (Array.isArray(raw)) {
                return { errors: raw, email: data.Email };
            }
            if (raw && typeof raw === "object") {
                return {
                errors: Object.values(raw).flat() as string[],
                email: data.Email,
                };
            }

            if (typeof raw === "string") {
                return { errors: [raw], email: data.Email };
            }
            return {
                errors: ["Login failed"],
                email: data.Email,
            };
        }
    }

    const [formState, formAction] = useActionState(submitAction,{});

    return (
    <Modal open={isLoginOpen} onClose={()=>navigate("/")}>
        <form className="login-form bg-stone-100 border-stone-950" action={formAction}>
            <Input label="Email" id="Email" type="email" defaultValue={formState?.email}/>
            <Input label="Password" id="Password" type="password"/>
            
            {formState.errors && (
                <ErrorContainer>
                    {
                    formState.errors.map(error => (  
                    <Error key={error} message={error}/>
                ))}
                </ErrorContainer>)}

            <p className="form-actions">
                <Button text="Cancel" className="px-2 text-stone-500" type="button" onClick={()=>navigate("/")} disabled={isPending}/>
                <Button text="Log In" className="login-form-button bg-stone-950 w-15 text-stone-300 mr-0.5 rounded-md" disabled={isPending}/>
            </p>
        </form>
    </Modal>)
}