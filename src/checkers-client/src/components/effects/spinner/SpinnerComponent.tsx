import {FC} from "react";
import styles from "./SpinnerComponent.module.css"

const SpinnerComponent: FC = (): JSX.Element => {
    return <div className={styles.loader}></div>
}

export default SpinnerComponent;