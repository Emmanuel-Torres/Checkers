import { FC } from "react";
import { GoogleLogout } from "react-google-login";
import { useDispatch } from "react-redux";
import { StoreDispatch } from "../../store";
import { logout } from "../../store/auth-slice";

const Logout: FC = (): JSX.Element => {
    const dispatch = useDispatch<StoreDispatch>();

    const clientId = process.env.REACT_APP_CLIENT_ID ?? "203576300472-3j2eeg1m35ahrg4ar8srm36ul8d504h5.apps.googleusercontent.com";

    const onFailure = () => {
        alert('Could not log you out');
    }

    const onLogoutSuccess = () => {
        dispatch(logout());
    }

    return (
        <>
            <GoogleLogout
                clientId={clientId}
                buttonText='Logout'
                onFailure={onFailure}
                onLogoutSuccess={onLogoutSuccess} />
        </>
    )
}

export default Logout;