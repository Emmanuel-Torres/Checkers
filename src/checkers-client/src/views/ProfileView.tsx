import { ChangeEvent, FC, FormEvent, useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import User from "../models/user";
import { StoreDispatch, useStoreSelector } from "../store";
import { authenticateUser, updateProfile } from "../store/auth-slice";

const ProfileView: FC = (): JSX.Element => {
    const dispatch = useDispatch<StoreDispatch>();
    const token = useStoreSelector(store => store.auth.userToken)
    const profile = useStoreSelector(store => store.auth.userProfile);

    const [joke, setJoke] = useState<string>("");
    const [iceCream, setIceCream] = useState<string>("");
    const [pizza, setPizza] = useState<string>("");
    const [age, setAge] = useState<number>(0);

    const isFormValid = (
        joke.trim().length > 0 &&
        iceCream.trim().length > 0 &&
        pizza.trim().length > 0 &&
        age > 0
    )

    useEffect(() => {
        dispatch(authenticateUser(token ?? ''));
    }, [dispatch, token]);

    const jokeChanged = (e: ChangeEvent<HTMLInputElement>) => {
        setJoke(e.target.value);
    }

    const iceCreamChanged = (e: ChangeEvent<HTMLInputElement>) => {
        setIceCream(e.target.value);
    }

    const pizzaChanged = (e: ChangeEvent<HTMLInputElement>) => {
        setPizza(e.target.value);
    }

    const ageChanged = (e: ChangeEvent<HTMLInputElement>) => {
        setAge(parseInt(e.target.value));
    }

    const submitForm = (e: FormEvent) => {
        e.preventDefault();

        if (isFormValid && profile && token) {
            const user = new User(profile.id, profile.givenName, profile.familyName, profile.picture, profile.email, joke, iceCream, pizza, age);
            dispatch(updateProfile({ token, user }))
        }
    }

    return (
        <>
            {!profile 
                ? <h2>You are not authenticated</h2>
                : <>
                    <h2>Profile information</h2>
                    <p>Email: {profile.email}</p>
                    <p>Name: {profile.givenName}</p>
                    <p>Joke: {profile.bestJoke}</p>
                    <p>Pizza: {profile.pizza}</p>
                    <p>Ice Cream: {profile.iceCreamFlavor}</p>
                    <p>Age: {profile.age}</p>

                    <form onSubmit={submitForm}>
                        <label>Joke</label>
                        <input type="text" onChange={jokeChanged} value={joke} />
                        <label>Ice Cream</label>
                        <input type="text" onChange={iceCreamChanged} value={iceCream} />
                        <label>Pizza</label>
                        <input type="text" onChange={pizzaChanged} value={pizza} />
                        <label>Age</label>
                        <input type="number" onChange={ageChanged} value={age} />
                        <button>submit</button>
                    </form>
                </>
            }
        </>
    )
}

export default ProfileView;