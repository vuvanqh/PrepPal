import Logo from '../../assets/logo.png';

const Navbar: React.FC = () => {
    return <nav className="navbar">
        <div>
            <h1>PrepPal+</h1>
            <img src={Logo}/>
        </div>
        <div>
            <button>Register</button>
            <button>Login</button>
        </div>
    </nav>
}

export default Navbar;