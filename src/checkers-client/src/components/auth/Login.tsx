import { FC } from "react";
import GoogleLogin, { GoogleLoginResponse } from "react-google-login";
import { useDispatch } from "react-redux"
import { StoreDispatch } from "../../store"
import { authenticateUser, setToken } from "../../store/auth-slice";
import { refreshTokenSetup } from "../../services/refresh-token";

const Login: FC = (): JSX.Element => {
    const dispatch = useDispatch<StoreDispatch>();
    const clientId: string = process.env.REACT_APP_CLIENT_ID ?? "203576300472-qleefq8rh358lkekh6c1vhq3222jp8nh.apps.googleusercontent.com";

    const onSuccess = (res: any) => {
        dispatch(setToken(res.tokenId));
        dispatch(authenticateUser(res.tokenId));
        console.log(res);

        // refreshTokenSetup(res);
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