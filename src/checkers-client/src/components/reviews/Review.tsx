import { FC } from "react";
import Review from "../../models/review";

type Props = {
    review: Review
}

const ReviewComponent: FC<Props> = (props): JSX.Element => {
    return <>
        <p>Author: {props.review.playerId}</p>
        <p>{props.review.content}</p>
    </>
}

export default ReviewComponent;