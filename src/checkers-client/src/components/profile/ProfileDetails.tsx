import { FC } from "react";
import User from "../../models/user";

type Props = {
    profile: User;
}

const ProfileDetails: FC<Props> = (props): JSX.Element => {
    return (
        <>
            <h2>Profile information</h2>
            <p>Email: {props.profile.email}</p>
            <p>Name: {props.profile.givenName}</p>
            <p>Joke: {props.profile.bestJoke}</p>
            <p>Pizza: {props.profile.pizza}</p>
            <p>Ice Cream: {props.profile.iceCreamFlavor}</p>
            <p>Age: {props.profile.age}</p>
            <img src={props.profile.picture} alt="profile" />
        </>
    )
}

export default ProfileDetails;