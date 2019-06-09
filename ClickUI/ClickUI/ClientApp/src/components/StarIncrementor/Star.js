import React, { Component } from "react";
import styles from "./Star.module.css";
import { TweenLite } from "gsap";
import { randomBetween } from "../../util/util";

export class Star extends Component {
    constructor(props) {
        super(props);
        this.button_starButtonClicked = this.button_starButtonClicked.bind(this);
    }

    button_starButtonClicked() {
        this.props.onClick();

        TweenLite.to(this.button, 0.1, { y: randomBetween(-2, 8) });
        TweenLite.to(this.button, 0.1, { x: randomBetween(-5, 5) });
        TweenLite.to(this.button, 0.3, { rotation: "+=" + randomBetween(-20, 20) });
    }

    wiggle() {
        TweenLite.to(this.button, 0.3, { rotation: "+=" + randomBetween(-5, 5) });
    }
    
    render() {
        return (
            <div className={styles.Container}>
                <div className={styles.StarContainer}>
                    <div className={styles.StarContainer__Star}>
                        <img alt="" src="/star.png" className={styles.StarContainer__Star__Button} onClick={this.button_starButtonClicked} ref={img => this.button = img} />
                    </div>
                </div>
            </div>
        );
    }
}