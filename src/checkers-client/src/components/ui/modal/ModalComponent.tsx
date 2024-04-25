import { FC } from "react";
import styles from "./ModalComponent.module.css";

type Props = {
    title: string;
    message: string;
    actionLabel?: string;
    closeLabel?: string;
    action?: () => void;
}

const ModalComponent: FC<Props> = (props): JSX.Element => {
    return <div>
        <h3>{props.title}</h3>
        <p>{props.message}</p>
    </div>
}

export default ModalComponent;