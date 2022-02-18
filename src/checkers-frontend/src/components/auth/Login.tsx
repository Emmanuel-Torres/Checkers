import { FC } from "react";
import GoogleLogin, { GoogleLoginResponse } from "react-google-login";
import { useDispatch } from "react-redux"
import { StoreDispatch } from "../../store"
import { authenticateUser } from "../../store/auth-slice";

const Login: FC = (): JSX.Element => {
    const dispatch = useDispatch<StoreDispatch>();
    const clientId: string = process.env.REACT_APP_CLIENT_ID ?? "203576300472-3j2eeg1m35ahrg4ar8srm36ul8d504h5.apps.googleusercontent.com";

    const onSuccess = (res: any) => {
        dispatch(authenticateUser(res.tokenId))
    }

    const onFailure = (res: GoogleLoginResponse) => {
        console.log('something went wrong', res);
    }

    return (
        <>
            <GoogleLogin
                clientId={clientId}
                buttonText='Login'
                onSuccess={onSuccess}
                onFailure={onFailure}
                isSignedIn={true}
                cookiePolicy='single_host_origin'
            />
        </>
    )
};

export default Login;