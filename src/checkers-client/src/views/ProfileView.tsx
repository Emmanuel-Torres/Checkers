import { FC, useEffect } from "react";
import { useDispatch } from "react-redux";
import ProfileDetails from "../components/profile/ProfileDetails";
// import ProfileUpdateRequest from "../models/proflie-update-request";
import { StoreDispatch, useStoreSelector } from "../store";
import { authenticateUser } from "../store/auth-slice";

const ProfileView: FC = (): JSX.Element => {
    const dispatch = useDispatch<StoreDispatch>();
    const token = useStoreSelector(store => store.auth.userToken)
    const profile = useStoreSelector(store => store.auth.userProfile);

    useEffect(() => {
        dispatch(authenticateUser(token ?? ''));
    }, [dispatch, token]);

    // const submitForm = (update: ProfileUpdateRequest) => {
    //     if (update && token) {
    //         dispatch(updateProfile({ update, token }));
    //     }
    // }

    return (
        <>{!profile
            ? <h2>You are not authenticated</h2>
            : <>
                <ProfileDetails profile={profile} />
            </>}
        </>
    )
}

export default ProfileView;