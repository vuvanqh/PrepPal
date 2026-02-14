    import Logo from '../../assets/logo.png';
    import { useNavigate  } from 'react-router-dom';

    const Navbar: React.FC = () => {
        const navigate = useNavigate();


        return <nav className="navbar">
            <div>
                <h1>PrepPal+</h1>
                <img src={Logo}/>
            </div>
            <div>
                <button onClick={()=>navigate("/register")}>Register</button>
                <button onClick={()=>navigate("/login")}>Login</button>
            </div>
        </nav>
    }

    export default Navbar;