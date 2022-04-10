import { ChangeEvent, FC, FormEvent, useState } from "react"
import UpdateProfileRequest from "../../models/update-proflie-request";
import User from "../../models/user";

type Props = {
    profile: User;
    onSubmit: (update: UpdateProfileRequest) => void;
}

const ProfileForm: FC<Props> = (props): JSX.Element => {
    const [joke, setJoke] = useState("");
    const [iceCream, setIceCream] = useState("");
    const [pizza, setPizza] = useState("");
    const [age, setAge] = useState(0);
    const [image, setImage] = useState<File>();

    const isFormValid = (
        joke.trim().length > 0 &&
        iceCream.trim().length > 0 &&
        pizza.trim().length > 0 &&
        age > 0
    )

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

    const fileChanged = (e: ChangeEvent<HTMLInputElement>) => {
        if (e.target.files && e.target.files[0]) {
            setImage(e.target.files[0]);
        }
    }

    const submitForm = (e: FormEvent) => {
        e.preventDefault();

        if (isFormValid) {
            props.onSubmit(new UpdateProfileRequest(joke, iceCream, pizza, age, image));
        }
    }

    return (
        <form onSubmit={submitForm}>
            <label>Joke</label>
            <input type="text" onChange={jokeChanged} value={joke} />
            <label>Ice Cream</label>
            <input type="text" onChange={iceCreamChanged} value={iceCream} />
            <label>Pizza</label>
            <input type="text" onChange={pizzaChanged} value={pizza} />
            <label>Age</label>
            <input type="number" onChange={ageChanged} value={age} />
            <label>Profile picture</label>
            <input type="file" onChange={fileChanged} />
            {image && <>
                <img alt="not fount" width={"250px"} src={URL.createObjectURL(image)} />
                <button onClick={() => setImage(undefined)}>Remove</button>
            </>}
            <button>submit</button>
        </form>
    )
}

export default ProfileForm;