import Input from "../components/UI/Input"
import Button from "../components/UI/Button"
import Modal from "../components/Modal";
import { useNavigate, useLocation } from "react-router-dom";
import {useMutation} from "@tanstack/react-query";
import { register } from "../api/account";
import { toast, Bounce, type ToastOptions } from 'react-toastify';
import { useActionState } from "react";
import type {registerDTO} from "../api/account";


type RegisterFormState = {
  errors?: string[];
  oldData?: registerDTO;
};

const toastConfig = {
    position: "top-right",
    autoClose: 5000,
    hideProgressBar: false,
    closeOnClick: true,
    pauseOnHover: true,
    draggable: true,
    progress: undefined,
    theme: "colored",
    transition: Bounce,
} as ToastOptions;


export default function Register(){
    const navigate = useNavigate();
    const isRegister = useLocation();
    const {mutateAsync } = useMutation({
        mutationFn: register,
        onSuccess: () => {
            navigate("/");
            toast.success("Registered Successfully!", toastConfig)
        },
    })

    async function submitAction(_: {errors?:string[]}, formData: FormData): Promise<RegisterFormState>{
        const data = {
            "FirstName": formData.get("FirstName") as string,
            "LastName": formData.get("LastName") as string,
            "Email": formData.get("Email") as string,
            "PhoneNumber": formData.get("PhoneNumber") as string,
            "Password": formData.get("Password") as string,
            "UserName": formData.get("UserName") as string
        };

        const errors: string[] = [];

        if(!data.Email.includes("@"))   
            errors.push("Invalid Email")

        if(data.Password!=formData.get("ConfirmPassword"))
            errors.push("Passwords do not match");

        if(data.PhoneNumber.length!=9)
            errors.push("Invlaid Phone Number")

        if(errors.length>0)
        {
            toast.error("Try Again :c", toastConfig);
            return {errors, oldData: data};
        }


        try {
            await mutateAsync(data);
            return {};
        } catch (err: any) {
            toast.error("Registration Failed. Try Again :c", toastConfig);

            const raw = err.info?.errors;

            // Case 1: backend sent string[]
            if (Array.isArray(raw)) {
                return { errors: raw, oldData: data };
            }

            // Case 2: backend sent { field: string[] }
            if (raw && typeof raw === "object") {
                return {
                errors: Object.values(raw).flat() as string[],
                oldData: data,
                };
            }

            // Case 3: backend sent single message
            if (typeof raw === "string") {
                return { errors: [raw], oldData: data };
            }

            // Fallback
            return {
                errors: ["Registration failed"],
                oldData: data,
            };
        }
    }

    const [formState,formAction] = useActionState(submitAction,{});

    return <Modal open={isRegister.pathname==='/register'} onClose={()=>navigate("/")}>
        <form className="register-form bg-stone-100" action={formAction}>
            <div className="form-actions">
                <Input label="First Name" id="FirstName" defaultValue={formState.oldData?.FirstName}/>
                <Input label="Last Name" id="LastName" defaultValue={formState.oldData?.LastName}/>
            </div>
        
            <Input label="Email" id="Email" type="email" defaultValue={formState.oldData?.Email}/>
            <Input label="Phone Number" id="PhoneNumber"defaultValue={formState.oldData?.PhoneNumber}/>
            <Input label="Username" id="UserName"defaultValue={formState.oldData?.UserName}/>
            <Input label="Password" id="Password"defaultValue={formState.oldData?.Password} type="password"/>
            <Input label="Confirm Password" id="ConfirmPassword" type="password"/>

            {formState.errors && (
            <ul>
                {
                formState.errors.map(error => (  
                <li key={error} className="text-red-950 size-1">
                    {error }
                </li>
            ))}
            </ul>)}

            <p className="form-actions">
                <Button text="Cancel" className="px-2 text-stone-500" type="button" onClick={()=> navigate("/")}/>
                <Button text="Register" className="bg-stone-950 w-20 text-stone-300 mr-0.5 rounded-md"/>
            </p>
        </form>
    </Modal>
}