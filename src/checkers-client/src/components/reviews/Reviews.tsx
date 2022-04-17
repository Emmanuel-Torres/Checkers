import { FC } from "react";
import Review from "../../models/review";
import ReviewComponent from "./ReviewComponent";

type Props = {
    reviews: Review[]
}

const Reviews: FC<Props> = (props): JSX.Element => {
    return <div>
        {props.reviews.map(r => <ReviewComponent key={r.id} review={r} />)}
    </div>
}

export default Reviews;