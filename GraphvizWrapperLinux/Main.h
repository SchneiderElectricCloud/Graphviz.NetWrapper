#pragma once

#ifndef main_h__
#define main_h__
extern "C" {

    // Some wrappers around existing cgraph functions to handle string marshaling
    const char* rj_agmemwrite(Agraph_t * g);
    Agraph_t* rj_agmemread(const char* s);
    const char* rj_agget(void* obj, char* name);
    const char* rj_agnameof(void* obj);
    Agraph_t* rj_agopen(char* name, int graphtype);
    const char *rj_sym_key(Agsym_t *sym);

    double node_x(Agnode_t* node);
    double node_y(Agnode_t* node);
    double node_width(Agnode_t* node);
    double node_height(Agnode_t* node);

    textlabel_t* node_label(Agnode_t* node);
    textlabel_t* edge_label(Agedge_t* edge);
    textlabel_t* graph_label(Agraph_t* graph);

    double label_x(textlabel_t* label);
    double label_y(textlabel_t* label);
    double label_width(textlabel_t* label);
    double label_height(textlabel_t* label);
    const char* label_text(textlabel_t* label);
    double label_fontsize(textlabel_t* label);
    const char* label_fontname(textlabel_t* label);

    void clone_attribute_declarations(Agraph_t* from, Agraph_t* to);
    void convert_to_undirected(Agraph_t *graph);


    bool echobool(bool arg);
    int echoint(int arg);
    void rj_debug();


}
#endif

