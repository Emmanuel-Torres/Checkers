import { FC } from "react";
import { GoogleLogout } from "react-google-login";

const Logout: FC = (): JSX.Element => {
    const clientId = process.env.REACT_APP_CLIENT_ID ?? "203576300472-3j2eeg1m35ahrg4ar8srm36ul8d504h5.apps.googleusercontent.com";

    const onFailure = () => {
        console.log("Failed to logout");
    }

    const onLogoutSuccess = () => {
        console.log("Logout successful");
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