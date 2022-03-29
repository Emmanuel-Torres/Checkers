import { FC } from "react";
import { NavLink } from "react-router-dom";

const Navbar: FC = (): JSX.Element => {
    return (
        <nav>
            <NavLink to='/'>Checker's</NavLink>
            <div>
                <NavLink to='/'>Home</NavLink>
            </div>
        </nav>
    )
}

export default Navbar;