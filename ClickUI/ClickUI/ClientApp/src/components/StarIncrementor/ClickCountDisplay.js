import React, { Component } from "react";
import { TweenLite } from "gsap";
import styles from "./ClickCountDisplay.module.css";
import { nil } from "../../util/util";

export class ClickCountDisplay extends Component {
    constructor(props) {
        super(props);

        this.bounceTotal = this.bounceTotal.bind(this);
    }

    componentWillReceiveProps(newProps) {
        if (newProps.total !== this.props.total) {
            this.bounceTotal(1.05);
        }
    }

    bounceTotal(bounciness) {
        TweenLite.to(this.total, 0.05, { scale: bounciness, onComplete: descendBack.bind(this) });
        function descendBack() {
            if (!nil(this.total)) {
                TweenLite.to(this.total, 0.05, { scale: 1 });
            }
        }
    }

    render() {
        return (
            <div className={styles.Container}>
                <div className={styles.ClickCounter}>
                    <div className={styles.ClickCounter__Total} ref={div => this.total = div}>
                        {this.props.total}
                    </div>
                    <div className={styles.ClickCounter__Personal} ref={div => this.personal = div}>
                        YOU: {this.props.personal}
                    </div>
                </div>
            </div>
        );
    }
}