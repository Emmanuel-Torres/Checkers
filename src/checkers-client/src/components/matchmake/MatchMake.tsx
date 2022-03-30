import { FC } from "react"

const MatchMake: FC = (): JSX.Element => {
    return (
        <>
            <div className='container border border-dark border-5 rounded p-2 my-2 shadow text-center'>
                <h1>Rules</h1>
                <ul className="text-start">
                    <li>TBD</li>
                </ul>

                <button type="button" className="btn btn-primary">Match make now!</button>
            </div>
        </>
    )
}

export default MatchMake;