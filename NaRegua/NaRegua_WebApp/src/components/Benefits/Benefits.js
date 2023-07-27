/* eslint-disable array-callback-return */
/* eslint-disable jsx-a11y/alt-text */
/* eslint-disable jsx-a11y/anchor-is-valid */
import React from 'react';
import './Benefits.css'

const Benefits = (props) => {
    return (
        <section className="caixa mb-5">
            <div className="container">
                <div className="row">
                    {
                        props.propertiesBenefits.map((item, index) => (
                            <div key={ index } className="col-md-4">
                                <img src={ item.image } className="img-fluid" />
                                <h4>{ item.title }</h4>
                                <p>
                                    { item.paragraph }
                                </p>
                            </div>
                        ))
                    }
                </div>
            </div>
        </section>
    );
}

export default Benefits;